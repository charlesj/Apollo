import apolloServer from '../../services/apollo-server';

function getQueueItems() {
  return apolloServer.invoke("getTodoQueueItems", {});
}

function updateItem(item) {
  return apolloServer.invoke("updateTodoQueueItem", {
    item
  });
}

function addItem(item) {
  return apolloServer.invoke("addTodoQueueItem", {
    item
  });
}

module.exports = {
  addItem,
  getQueueItems,
  updateItem
};
