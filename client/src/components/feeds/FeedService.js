import localStorage from 'store';
import li from 'lorem-ipsum';

const key = "feeds";
const itemKey = "feedItems";

var feeds = [
  {
    id: 1,
    name: "Test Feed 1",
    unreadCount: 0,
  },
  {
    id: 2,
    name: "Another Test Feed ",
    unreadCount: 0,
  }
];

if (localStorage.get(key) == null) {
  localStorage.set(key, feeds);
}

var generateItem = (feedId, itemId) => {
  var title = li({
    count: 2,
    units: 'words'
  });
  var body = li({
    count: 3,
    units: 'paragraphs'
  });
  var link = "http://example.com";
  return {
    id: itemId,
    title,
    body,
    link,
    read: false,
    feed_id: feedId
  };
}

var getFeeds = () => {
  return new Promise((res, rej) => {
    res(localStorage.get(key));
  });
}

var getItems = (feedId) => {
  var items = localStorage.get(itemKey);
  var feeds = localStorage.get(key);
  if (items == null) {
    items = [];
  }
  var currentLastId = items.reduce((p, c) => {
    if (p.id > c.id) {
      return p.id;
    }
    return c.id;
  }, 0);

  if (items.filter(f => {
      return f.read;
    }).length <= 5) {
    for (var i = 0; i < 10; i++) {
      var feedIndex = Math.floor(Math.random() * (feeds.length - 1));
      console.log(currentLastId);
      items.push(generateItem(feeds[feedIndex].id, currentLastId++));
    }
  }

  localStorage.set(itemKey, items);

  return new Promise((res, rej) => {
    res(items.filter(item => {
      return (feedId === -1 || item.feed_id === feedId) && !item.read;
    }));
  });
}

var markItemAsRead = (itemId) => {
  var items = localStorage.get(itemKey);
  items.foreach(item => {
    if (item.id === itemId) {
      item.read = true;
    }
    ;
  });
  return new Promise((res, rej) => {
    res();
  });
}

module.exports = {
  getFeeds,
  getItems,
  markItemAsRead
}
