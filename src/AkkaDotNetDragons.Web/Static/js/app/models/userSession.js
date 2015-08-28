// -----------------------------------------------------------------------
// <copyright file="userSession.js">
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
define(['jquery','cookies','utils'], function($, $cookies, $utils) {

    var instance;

    // Constructor
    function UserSession() {
        this.SESSION_ID = 'hero';
        this.SESSION_NAME = 'hero_name';
        this.SESSION_HASH = 'hero_hash';
        this.utils = $utils.getInstance();
    }
                
    Object.defineProperty(UserSession.prototype, 'id', {
        get: function() {
            return $cookies.get(this.SESSION_ID);
        },
        set: function(val) {
            if (val === '') {
                $cookies.remove(this.SESSION_ID);
            } else {
              $cookies.set(this.SESSION_ID, val);  
            }
        }
    });

    Object.defineProperty(UserSession.prototype, 'fullname', {
        get: function() {
            return $cookies.get(this.SESSION_NAME);
        },
        set: function(val) {
            if (val === '') {
                $cookies.remove(this.SESSION_NAME);
            } else {
                $cookies.set(this.SESSION_NAME, val);
            }
        }
    });

    Object.defineProperty(UserSession.prototype, 'hash', {
        get: function() {
            return $cookies.get(this.SESSION_HASH);
        },
        set: function(val) {
            if (val === '') {
                $cookies.remove(this.SESSION_HASH);
            } else {
                $cookies.set(this.SESSION_HASH, val);
            }
        }
    });

    Object.defineProperty(UserSession.prototype, 'isDefined', {
        get: function() {
            return (typeof this.id !== 'undefined') && this.id !== null && this.id !== '';
        }
    });

    UserSession.prototype.create = function(name, hash) {
        this.id = this.utils.slugify(name);
        this.fullname = name;
        this.hash = hash;
    };

    UserSession.prototype.clear = function() {
        this.id = '';
        this.fullname = '';
        this.hash = '';
    };

    return {
        getInstance: function() {
            if (!instance) {
                instance = new UserSession();
            }
            return instance;
        }
    };

});