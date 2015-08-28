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
define(['jquery','cookies','eventsource','handlebars','machina','md5','usersession','utils','skylo','ssutils','text!views/joinForm.html','text!views/infoForm.html','text!views/actionForm.html','text!views/buttons/attackBtn.html','text!views/buttons/defendBtn.html','text!views/buttons/healBtn.html'],
    function($, $cookies, $source, $handlebars, $machina, $md5, $session, $utils, $skylo, $ssutils, $joinFormTpl, $infoFormTpl, $actionFormTpl, $attackBtnTpl, $defendBtnTpl, $healBtnTpl) {

        // Elements references
        var envCtr = $('body #environment');
        var resultCtr = $('body .result');
        var actionCtr, infoCtr, joinCtr, joinForm;

        // Dragons
        var dragons = [];

        // Template
        var attackTpl = $handlebars.compile($attackBtnTpl);

        // Event source
        var source = new $source('/event-stream?channel=player');

        var instance = new $machina.Fsm({
            namespace: 'playerEnv',
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
                                    dragons = m.dragons;
                                    instance.transition(m.status.toLowerCase());
                                },
                                GetReadyMessage: function(m, e) {
                                    dragons = m.dragons;
                                    instance.setActionForm('refresh');
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
                        instance.setJoinForm('create');
                        instance.setInfoForm('create');
                        instance.setActionForm('create');

                        $.skylo('end');
                    }
                },
                ready: {
                    _onEnter: function() {
                        instance.refresh();
                    },
                    _start: function() {
                        instance.transition('registering');
                    }
                },
                registering: {
                    _onEnter: function() {
                        instance.refresh();
                        $('#footer .title').removeClass('init');
                    }
                },
                active: {
                    _onEnter: function() {
                        instance.refresh();
                        $('#footer .title').removeClass('init');

                        // Lock the player change button, and
                        // show the game commands
                        instance.setInfoForm('lock');
                        actionCtr.show();
                    },
                    _refresh: function() {
                        
                    },
                    _onExit: function() {
                        actionCtr.hide();
                        instance.setInfoForm('unlock');
                    }
                },
                finished: {
                    _onEnter: function() {
                        instance.refresh();
                        $('#footer .title').removeClass('init');
                    }
                }
            },

            // Function adapters
            create: function() { this.handle('_create'); },
            refresh: function() {
                instance.setJoinForm('refresh');
                instance.setInfoForm('refresh');
                instance.setActionForm('refresh');
            },
            start: function() { this.handle('_start'); },

            // Functions
            setJoinForm: function(method) {
                switch(method) {
                    case 'create':
                        envCtr.append($joinFormTpl);
        
                        // Join container declaration and event binding
                        joinCtr = envCtr.find('.join');
                        joinForm = joinCtr.find('form');
                        joinForm.submit(function(event) {
                            console.log('[evt]: user joins');

                            event.preventDefault();
                
                            var session = $session.getInstance();
                            var utils = $utils.getInstance();
                            var form = utils.formData(joinForm);
                            var fullname = form.fullname;
                            var email = form.email;

                            var slug = utils.slugify(fullname);
                            var userHash = $md5(email);

                            $.post('/environment/players/' + slug, { hash: userHash, name: fullname })
                                .done(function(data) {
                                    session.create(fullname, userHash);
                                    instance.refresh();
                                })
                                .fail(function(data) {
                                    window.alert("Duplicate player ID.");
                                });
                        });
                        break;
                        
                    case 'refresh':
                        var session = $session.getInstance();
                        if (session.isDefined) {
                            var nameField = joinForm.children('input:text[name=fullname]');
                            if ((typeof nameField.val() === 'undefined') || nameField.val() === null || nameField.val() === '') {
                                nameField.val(session.fullname);
                            }

                            joinCtr.hide();
                        } else {
                            joinCtr.show();
                        };
                        break;
                }
            },
            setInfoForm: function(method) {
                switch(method) {
                    case 'create':
                        envCtr.append($infoFormTpl);

                        // Info container declaration and event binding
                        infoCtr = envCtr.find('.info');
                        infoCtr.find('.change').on('click', function() {
                            console.log('[evt]: changing user details');

                            var session = $session.getInstance();

                            $.ajax({
                                url: '/environment/players/' + session.id,
                                type: 'DELETE'
                            }).done(function(data) {
                                session.clear();
                                instance.refresh();
                            });
                        });
                        break;

                    case 'refresh':
                        var session = $session.getInstance();
                        if (session.isDefined) {
                            infoCtr.find('p.name').text(session.fullname);
                            infoCtr.find('p.avatar').text('@' + session.id);
                            infoCtr.find('img').attr('src', 'http://www.gravatar.com/avatar/' + session.hash + '?s=128&r=pg');
                            infoCtr.show();
                        } else {
                            infoCtr.hide();
                        }
                        break;

                    case 'lock':
                        infoCtr.find('button.change').attr('disabled', 'disabled');
                        break;

                    case 'unlock':
                        infoCtr.find('button.change').removeAttr('disabled');
                        break;
                }
            },
            setActionForm: function(method) {
                switch(method) {
                    case 'create':
                        envCtr.append($actionFormTpl);

                        // Action container declaration and event binding
                        actionCtr = envCtr.find('.action');
                        actionCtr.find('.logout').on('click', function() {
                            console.log('[evt]: user logs out');

                            var session = $session.getInstance();
                            session.clear();
                            instance.refresh();
                        });
                        break;

                    case 'refresh':
                        // Detach event handler and removes the existing content
                        actionCtr.find('.attack').off('click');
                        actionCtr.find('.defend').off('click');
                        actionCtr.find('.heal').off('click');
                        actionCtr.remove();

                        // Re-add content
                        envCtr.append($actionFormTpl);
                        actionCtr = envCtr.find('.action');

                        // Add defend and heal buttons, and
                        // bind the buttons with click events
                        actionCtr.find('#defend').append($defendBtnTpl);
                        actionCtr.find('#defend > .defend').on('click', function() {
                            console.log('[evt]: user defends');

                            var session = $session.getInstance();

                            $.post('/action/defend', { user: session.id });
                        });
                        actionCtr.find('#heal').append($healBtnTpl);
                        actionCtr.find('#heal > .heal').on('click', function() {
                            console.log('[evt]: user heals');

                            var session = $session.getInstance();

                            $.post('/action/heal', { user: session.id });
                        });

                        // Add the dragon information
                        var i = 0;
                        console.log(dragons);
                        $.each(dragons, function(idx, val) {
                            var dragonId = val;
                            var dragonName = val.substr(0, 1).toUpperCase() + val.substr(1);
                            var context = { id: dragonId, name: dragonName }
                            actionCtr.find('#attack-' + i).append(attackTpl(context));
                            i++;
                        });
                                    
                        // Bind attack button with click event
                        actionCtr.find('.attack').on('click', function() {
                            console.log('[evt]: user attacks');

                            var session = $session.getInstance();

                            $.post('/action/attack', { user: session.id, target: $(this).attr('id') });
                        });
                        break;
                }
            }
        });

        return { getInstance: function() { return instance; } };

    }
);