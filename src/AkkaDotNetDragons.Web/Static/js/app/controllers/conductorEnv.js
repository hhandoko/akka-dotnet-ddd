// -----------------------------------------------------------------------
// <copyright file="conductorEnv.js">
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
define(['jquery','cookies','eventsource','machina','usersession','utils','skylo','ssutils'],
    function($, $cookies, $source, $machina, $session, $utils, $skylo, $ssutils) {

        // Elements references
        var actionCtr = $('#action-ctr');
        var resetCtr = $('#reset-ctr');

        // Action and reset button
        var actionBtn, resetBtn;

        // Event source
        var source = new $source('/event-stream?channel=conductor');

        var instance = new $machina.Fsm({
            namespace: 'conductorEnv',
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
                                }
                            }
                        });

                        instance.deferUntilTransition();
                        instance.transition('creating');
                    }
                },
                creating: {
                    _create: function() {
                        console.log("[env]: creating conductor environment");

                        $.skylo('set', 75);

                        $.get('/environment').done(function(data) {
                            instance.transition(data.status.toLowerCase());
                        });
                    },
                    _onExit: function() {
                        instance.setActionBtn('create');
                        instance.setResetBtn('create');

                        $.skylo('end');
                    }
                },
                ready: {
                    _onEnter: function() {
                        instance.setActionBtn('refresh');
                        instance.setResetBtn('refresh');
                        instance.setResetBtn('disable');
                        instance.setStatus('ready');
                    },
                    _start: function() {
                        instance.transition('registering');
                    }
                },
                registering: {
                    _onEnter: function() {
                        instance.setActionBtn('disable');
                        instance.setResetBtn('disable');
                        instance.setStatus('registering');
                    }
                },
                active: {
                    _onEnter: function() {
                        instance.setActionBtn('disable');
                        instance.setResetBtn('refresh');
                        instance.setStatus('active');
                    }
                },
                finished: {
                    _onEnter: function() {
                        instance.setActionBtn('disable');
                        instance.setResetBtn('refresh');
                        instance.setStatus('finished');
                    }
                }
            },

            // Function adapters
            create: function() { instance.handle('_create'); },

            // Functions
            setActionBtn: function(method) {
                switch(method) {
                    case 'create':
                        actionCtr.append('<button id="action" class="btn btn-circle btn-main btn-material-green" data-action="start"><i class="fa fa-play fa-3x"></i> <span class="sr-only">Start</span></button>');
                        actionBtn = actionCtr.find('button#action');
                        actionBtn.on('click', function() {
                            $.post('/environment/start').done(function(data) {
                                instance.handle('_start');
                            });
                        });
                        break;

                    case 'refresh':
                        actionBtn.off('click');
                        actionBtn.remove();
                        instance.setActionBtn('create');
                        break;

                    case 'disable':
                        actionBtn.off('click');
                        resetBtn.addClass('btn-material-grey-900').removeClass('btn-material-green');
                        actionBtn.attr('disabled', true);
                        break;
                }
            },
            setResetBtn: function(method) {
                switch(method) {
                    case 'create':
                        resetCtr.append('<button id="reset" class="btn btn-fab btn-material-red" data-action="reset"><i class="fa fa-power-off"></i><span class="sr-only">Reset</span></button>');
                        resetBtn = resetCtr.children('button#reset');
                        resetBtn.on('click', function() {
                            $.post('/environment/reset').done(function(data) {
                                instance.setResetBtn('disable');
                            });
                        });
                        break;
                        
                    case 'refresh':
                        resetBtn.off('click');
                        resetBtn.remove();
                        instance.setResetBtn('create');
                        break;

                    case 'disable':
                        resetBtn.off('click');
                        resetBtn.addClass('btn-material-grey-900').removeClass('btn-material-red');
                        resetBtn.attr('disabled', true);
                        break;

                }
            },
            setStatus: function(status) {
                var ACTIVE_BTN_CSS = 'btn-active';
                $('.statuses .' + ACTIVE_BTN_CSS).removeClass(ACTIVE_BTN_CSS);
                $('.statuses #status-' + status.toLowerCase()).addClass(ACTIVE_BTN_CSS);
            }
        });

        return { getInstance: function() { return instance; } };

    }
);