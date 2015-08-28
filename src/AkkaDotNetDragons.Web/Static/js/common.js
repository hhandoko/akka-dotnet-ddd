// -----------------------------------------------------------------------
// <copyright file="common.js">
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

require.config({
    baseUrl: 'static/js/app',
    paths: {
        // Core libraries and plugins
        bootstrap: '../../lib/bootstrap/js/bootstrap.min',
        cookies: '../../lib/js-cookie/js.cookie.min',
        eventsource: '../../lib/polyfills/EventSource.min',
        handlebars: '../../lib/handlebars/handlebars',
        jquery: '../../lib/jquery/jquery-1.11.3.min',
        lodash: '../../lib/lodash/lodash.min',
        machina: '../../lib/machina/machina.min',
        material: '../../lib/bootstrap-material/js/material.min',
        md5: '../../lib/md5/md5.min',
        ripples: '../../lib/bootstrap-material/js/ripples.min',
        skylo: '../../lib/skylo/js/skylo.min',
        ssutils: '../../lib/servicestack/ss-utils.min',
        text: '../../lib/requirejs/text.min',
        // App
        conductorenv: 'controllers/conductorEnv',
        playerenv: 'controllers/playerEnv',
        spectatorenv: 'controllers/spectatorEnv',
        usersession: 'models/userSession.min',
        utils: 'utils.min'
    },
    shim: {
        'bootstrap': {
            deps: ['jquery']
        },
        'cookies': {
            deps: ['jquery'],
            exports: 'Cookies'
        },
        'eventsource': {
            exports: 'EventSource'
        },
        'jquery': {
            exports: '$'
        },
        'material': {
            deps: ['bootstrap','jquery'],
            exports: '$.material'
        },
        'ripples': {
            deps: ['bootstrap','jquery']
        },
        'skylo': {
            deps: ['jquery']
        },
        'ssutils': {
            deps: ['jquery'],
            exports: '$.ss'
        }
    }
});