// -----------------------------------------------------------------------
// <copyright file="playerEnv.js">
//   Copyright (c) 2015 Akka.NET Dragons Demo contributors
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

// User session object
define(['jquery','cookies','eventsource','handlebars','machina','usersession','utils','skylo','ssutils','text!views/chat/leftBubble.html','text!views/chat/rightBubble.html'],
    function($, $cookies, $source, $handlebars, $machina, $session, $utils, $skylo, $ssutils, $leftBubbleTpl, $rightBubbleTpl) {

        // Elements references
        var envCtr = $('body #environment');

        // Template
        var botWarningIdx = 0;
        var botWarnings = [
            'Uhmm, what are you doing here?',
            'Why are more people joining in?',
            'No, I\'m serious, you guys should not be in this demo!',
            'I give up, don\'t say I didn\'t warn you...'
        ];
        var botAvatar = '/static/img/robot_128.png';
        var leftBubbleTpl = $handlebars.compile($leftBubbleTpl);
        var rightBubbleTpl = $handlebars.compile($rightBubbleTpl);

        var dragons = [];

        // Event source
        var source = new $source('/event-stream?channel=spectator');

        var instance = new $machina.Fsm({
            namespace: 'spectatorEnv',
            initialState: 'starting',
            states: {
                starting: {
                    '*': function() {
                        $.skylo('set', 50);

                        source.addEventListener('message', function(event) {
                            console.log('[msg]: ' + event.data);
                        });

                        source.addEventListener('open', function(e) {
                            console.log('[opn]: ' + e);
                        }, false);

                        source.addEventListener('error', function(e) {
                            console.log('[err]: ' + e);
                        }, false);

                        $(source).handleServerEvents({
                            handlers: {
                                EnvironmentStatusMessage: function(m, e) {
                                    instance.transition(m.status.toLowerCase());
                                },
                                GetReadyMessage: function(m, e) {
                                    instance.transition('registering');
                                },
                                String: function(m, e) {
                                    instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: m }));
                                },
                                SpectatorMessage: function(m, e) {
                                    switch(m.messageType) {
                                        case 'System':
                                            instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: m.message }));
                                            break;
                                        case 'Player':
                                            var playerAvatar = 'http://www.gravatar.com/avatar/' + m.avatar + '?s=128&r=pg';
                                            instance.print(leftBubbleTpl({ avatar_url: playerAvatar, message: m.message }));

                                            // Insert joke here...
                                            instance.handle('_joke', m.message);
                                            break;
                                        case 'Dragon':
                                            instance.print(rightBubbleTpl({ avatar_url: '/static/img/dragon-' + m.name + '_128.png', message: m.message }));
                                            break;

                                    }
                                }
                            }
                        });

                        instance.deferUntilTransition();
                        instance.transition('creating');
                    }
                },
                creating: {
                    _create: function() {
                        console.log("[env]: creating game environment");
                        
                        $.skylo('set', 75);

                        $.get('/environment').done(function(data) {
                            dragons = data.dragons;
                            instance.transition(data.status.toLowerCase());
                        });
                    },
                    _onExit: function() {
                        instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: 'Welcome to the Akka.NET DDD demo. Here be dragons!' }));

                        $.skylo('end');
                    }
                },
                ready: {
                    _onEnter: function() {
                        botWarningIdx = 0;
                    },
                    _joke: function(message) {
                        if (message.endsWith('has joined the demo') && botWarningIdx < 4) {
                            var idx = botWarningIdx;
                            setTimeout(function() {
                                instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: botWarnings[idx] }));
                            }, 5000);
                            botWarningIdx++;
                        }
                    }
                },
                registering: {
                    _onEnter: function() {
                        instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: 'Uh, oh... I can feel something bad is about to happen' }));

                        // Notify that the opponents have entered the arena
                        setTimeout(function() {
                            var i = 1;
                            $.each(dragons, function(idx, val) {
                                instance.print(rightBubbleTpl({ avatar_url: '/static/img/dragon-' + val + '_128.png', message: '`' + val + '` has entered the arena' }));
                                i++;
                            });
                        }, 3000);;

                        setTimeout(function() {
                            instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: 'Yikes!' }));
                        }, 7000);
                    }
                },
                active: {
                    _onEnter: function() {
                        instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: 'I\'m going to hide somewhere safe... :S' }));
                    },
                    _onExit: function() {
                        instance.print(leftBubbleTpl({ avatar_url: botAvatar, message: 'Glad that\'s over... Phew' }));
                    }
                },
                finished: {
                    
                }
            },

            // Function adapters
            create: function() { this.handle('_create'); },
            print: function(content) {
                envCtr.prepend(content);
                envCtr.children('div').first().addClass('show');
            }
        });

        return { getInstance: function() { return instance; } };

    }
);