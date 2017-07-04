var apolloServer = require('./apollo-server');

function getTodos() {
  return apolloServer.invoke("getTodoItems", {});
}

function toggleCompleted(item) {
  if (item.completed_at) {
    item.completed_at = null;
  } else {
    item.completed_at = new Date();
  }

  return apolloServer.invoke("updateTodoItem", {item});
}

function addItem(title) {
  return apolloServer.invoke("addTodoItem", {title});
}

module.exports = {
  getTodos: getTodos,
  toggleCompleted: toggleCompleted,
  addItem: addItem
}
