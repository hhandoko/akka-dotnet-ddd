// -----------------------------------------------------------------------
// <copyright file="utils.js">
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

define([], function() {

    var instance;

    // Constructor
    function Utils() {}
    
    // Slugify - convert input into URL-friendly format
    // Source: https://gist.github.com/mathewbyrne/1280286
    Utils.prototype.slugify = function(text) {
        return text.toString()
            .toLowerCase()
            .replace(/\s+/g, '-') // Replace spaces with -
            .replace(/[^\w\-]+/g, '') // Remove all non-word chars
            .replace(/\-\-+/g, '-') // Replace multiple - with single -
            .replace(/^-+/, '') // Trim - from start of text
            .replace(/-+$/, ''); // Trim - from end of text
    };
    
    // Return a key-value pair from form data.
    // Source: http://stackoverflow.com/a/24012884/1615437
    Utils.prototype.formData = function(form) {
        return form.serializeArray()
            .reduce(function(obj, item) {
                obj[item.name] = item.value;
                return obj;
            }, {});
    };
    
    return {
        getInstance: function() {
            if (!instance) {
                instance = new Utils();
            }
            return instance;
        }
    };

});