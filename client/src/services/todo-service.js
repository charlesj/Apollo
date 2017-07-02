var store = require('store');


function getTodos() {
  if (store.get('todoItems') == null) {
    store.set('todoItems', [{
      id: 1,
      title: "First"
    }, {
      id: 2,
      title: "Second"
    }]);
  }
  return new Promise((resolve, reject) => {
    resolve(store.get('todoItems'));
  });
}

function toggleCompleted(item) {
  var currentItems = store.get('todoItems');
  var newItems = [];
  currentItems.forEach((p) => {
    if (item.id === p.id) {
      if (p.completed_at) {
        p.completed_at = null;
      } else {
        p.completed_at = new Date();
      }
    }
    newItems.push(p);
  });
  console.log(newItems);
  store.set('todoItems', newItems);
  return new Promise((resolve, reject) => {
    resolve();
  });
}

function addItem(title) {
  var currentItems = store.get('todoItems');
  var lastId = 0;
  currentItems.forEach((i) => {
    if (i.id > lastId) {
      lastId = i.id;
    }
  });

  currentItems.push({
    id: lastId + 1,
    title: title
  });
  store.set('todoItems', currentItems);
  return new Promise((resolve, reject) => {
    resolve();
  });
}

module.exports = {
  getTodos: getTodos,
  toggleCompleted: toggleCompleted,
  addItem: addItem
}
