// Copyright (c) 2014 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

/**
 * Get the current URL.
 *
 * @param {function(string)} callback - called when the URL of the current tab
 *   is found.
 */

var endpoint = "https://apollo.sudolife.org/api";
var requestCounter = 0;

function getCurrentTabUrl(callback) {
  // Query filter to be passed to chrome.tabs.query - see
  // https://developer.chrome.com/extensions/tabs#method-query
  var queryInfo = {
    active: true,
    currentWindow: true
  };

  chrome.tabs.query(queryInfo, function(tabs) {
    // chrome.tabs.query invokes the callback with a list of tabs that match the
    // query. When the popup is opened, there is certainly a window and at least
    // one tab, so we can safely assume that |tabs| is a non-empty array.
    // A window can only have one active tab at a time, so the array consists of
    // exactly one tab.
    var tab = tabs[0];

    // A tab is a plain object that provides information about the tab.
    // See https://developer.chrome.com/extensions/tabs#type-Tab
    var url = tab.url;
    var title = tab.title;
    // tab.url is only available if the "activeTab" permission is declared.
    // If you want to see the URL of other tabs (e.g. after removing active:true
    // from |queryInfo|), then the "tabs" permission is required to see their
    // "url" properties.
    console.assert(typeof url == 'string', 'tab.url should be a string');

    callback(url, title);
  });

  // Most methods of the Chrome extension APIs are asynchronous. This means that
  // you CANNOT do something like this:
  //
  // var url;
  // chrome.tabs.query(queryInfo, function(tabs) {
  //   url = tabs[0].url;
  // });
  // alert(url); // Shows "undefined", because chrome.tabs.query is async.
}

function toggleDisplay(elementId, show){
    var element = document.getElementById(elementId);
    if(show === true ){
        element.style.display = 'block';
    } else {
        element.style.display = 'none';
    }
}

function checkLogin(){
    chrome.storage.sync.get('token', function(storage){
       if(storage.token){
           toggleDisplay('addBookmarkForm', true);
           toggleDisplay('loginForm', false);
       }else{
           toggleDisplay('addBookmarkForm', false);
           toggleDisplay('loginForm', true);
       }
    });
}

function getToken(callback){
    chrome.storage.sync.get('token', function(storage){
        callback(storage.token);
    });
}

function setToken(token, callback){
    chrome.storage.sync.set({'token': token}, function(){
        printToken();
        callback();
    });
}

function clearToken(callback){
    chrome.storage.sync.remove('token', callback);
}

function printToken(){
    getToken(function(token){
       console.log('token :', token);
    });
}

function login(){
    var password = document.getElementById('password').value;
    apolloRPC('login', {
        password: password
    }, function(result){
        if(result.token){
            console.log('logging in');
            setToken(result.token, function(){
                checkLogin();
            });
        }
    })
}

function apolloRPC(command, payload, callback){
    showLoading();
    getToken(function(token){
        payload.token = token;
        console.log(payload);
        var ajaxRequest = new XMLHttpRequest();
        ajaxRequest.open("POST", endpoint);
        ajaxRequest.setRequestHeader("Content-Type", "application/json");
        ajaxRequest.onreadystatechange = function() {
            if (ajaxRequest.readyState == 4) {
                hideLoading();
                console.log('Received Respnose', ajaxRequest);
                var resp = JSON.parse(ajaxRequest.responseText);
                callback(resp.result.Result);
            }
        }
        ajaxRequest.send(JSON.stringify({id: 'bk' + (requestCounter++).toString(), params: payload, method: command}));
    })
}

function logout(){
    getToken(function(token){
        apolloRPC('revokeLoginSession', {
            tokenToRevoke: token
        }, function(result){
            console.log('logging out');
            clearToken(function(){
               printToken();
               checkLogin();
            });
        });
    });
}

function submitBookmark(){
    var link = document.getElementById('pageUrl').value;
    var title = document.getElementById('pageTitle').value;
    var description = document.getElementById('description').value;
    var rawTags = document.getElementById('tags').value;
    var tags = [];
    if(rawTags){
        tags = rawTags.split(',');
    }

    apolloRPC('addbookmark',{
        title: title,
        link: link,
        description: description,
        tags: tags
    }, function(){
        toggleDisplay('success', true);
    });
}

function showLoading(){
    toggleDisplay('loading', true);
}

function hideLoading(){
    toggleDisplay('loading', false);
}

document.addEventListener('DOMContentLoaded', function() {
  document.getElementById("logout").addEventListener("click", logout);
  document.getElementById("login").addEventListener("click", login);
  document.getElementById("submitBookmark").addEventListener("click", submitBookmark);
});

document.addEventListener('DOMContentLoaded', function() {
  toggleDisplay('success', false);
  checkLogin();
  getCurrentTabUrl(function(url, title) {
    document.getElementById('pageUrl').value = url;
    document.getElementById('pageTitle').value = title;
  });
});
