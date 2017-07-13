import store from 'store';

var dataKey = "queueItems";

function getQueueItems(){
  var data = store.get(dataKey);
  if(data == null){
    store.set(dataKey, [
      {
        id: 1,
        title: "here is title",
        link: "http://cnn.com",
        description: "here is my description",
        created_at: new Date(),
        completed_at: null,
      },
      {
        id: 2,
        title: "Second Queue Item",
        link: "http://example.com",
        description: "no real description",
        created_at: new Date(),
        completed_at: null,
      },
    ]);
    data = store.get(dataKey);
  };

  return new Promise((resolve) => {
    resolve(data.filter(d => {
      return d.completed_at === null;
    }));
  });
}

function updateItem(item){
  var currentItems = store.get(dataKey);
  var newItems = [];
  currentItems.forEach(i => {
    if(i.id === item.id){
      newItems.push(item);
    } else{
      newItems.push(i);
    }
  });

  store.set(dataKey, newItems);

  return new Promise((resolve) => {
    resolve();
  });
}

function addItem(item){
  var currentItems = store.get(dataKey);
  item.id = currentItems.length + 1;
  currentItems.push(item);
  store.set(dataKey, currentItems);
  console.log(currentItems);
  return new Promise((resolve) => {
    resolve();
  });
}

module.exports = {
  addItem,
  getQueueItems,
  updateItem
};
