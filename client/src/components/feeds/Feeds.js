import React from 'react';
import HotKey from 'react-shortcut';
import FeedItems from './FeedItems';
import FeedService from './FeedService';

function getFeedClasses(selectedId, feedId) {
  if (selectedId !== feedId) {
    return "feed";
  }

  return "feed activeFeed"
}

function FeedDisplay(props) {

  return (<div className='feedsContainer'>
    <div className={getFeedClasses(props.selectedId, -1)} onClick={props.changeFeed.bind(null, -1)}>All Items ({ props.feeds.reduce((p, n) => {
      return p + n.unread_count;
    }, 0)})</div>
    { props.feeds.map(f => {
      return (<div className={getFeedClasses(props.selectedId, f.id)} key={f.id} onClick={props.changeFeed.bind(null, f.id)}>{f.name} ({f.unread_count})</div>)
    })}
  </div>)
}

class Feeds extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      feeds: [],
      selectedFeedId: -1,
      previousItems: [],
      nextItems: [],
      currentItem: null
    };

    this.setFeed = this.setFeed.bind(this);
    this.changeSelectedFeed = this.changeSelectedFeed.bind(this);
    this.moveNextItem = this.moveNextItem.bind(this);
    this.movePreviousItem = this.movePreviousItem.bind(this);
    this.openItem = this.openItem.bind(this);
  }

  componentDidMount() {
    this.setFeed(-1);
  }

  setFeed(feedId) {
    FeedService.getFeeds().then((feeds) => {
      FeedService.getItems(feedId)
        .then((items) => {
          if (items.length > 0) {
            this.setState({
              feeds,
              nextItems: items.slice(1),
              currentItem: items.slice(0, 1)[0],
              selectedFeedId: feedId
            });
          } else {
            this.setState({
              feeds,
              nextItems: [],
              previousItems: [],
              selectedFeedId: feedId,
              currentItem: null
            });
          }
        });
    });
  }

  changeSelectedFeed(feedId) {
    this.setFeed(feedId);
  }

  moveNextItem() {
    if (this.state.nextItems.length === 0) {
      return;
    }

    var shouldAdd = (items, previousItems, newItem) => {
      return this.state.currentItem.id !== newItem.id &&
            previousItems.filter(item => item.id === newItem.id).length === 0 &&
             items.filter(item => item.id === newItem.id).length === 0;
    };

    if (this.state.nextItems.length <= 5) {
      FeedService.getItems(this.state.selectedFeedId)
        .then((items) => {
          var newNextItems = this.state.nextItems;
          for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (shouldAdd(newNextItems,this.state.previousItems, item)) {
              newNextItems.push(item);
            }
          }
          this.setState({
            nextItems: newNextItems
          });
        });
    }

    var currentItem = this.state.currentItem;
    if (currentItem.read_at === null) {
      FeedService.markItemAsRead(currentItem.id);
      currentItem.read_at = new Date();
      var feeds = this.state.feeds;
      feeds.forEach(f => {
        if(f.id === currentItem.feed_id){
          f.unread_count--;
        }
      });
      this.setState({feeds});
    }

    var newPreviousItems = this.state.previousItems;
    newPreviousItems.push(currentItem);
    var newCurrent = this.state.nextItems.slice(0, 1)[0];
    if (newPreviousItems.length > 5) {
      newPreviousItems = newPreviousItems.slice(1);
    }

    this.setState({
      previousItems: newPreviousItems,
      currentItem: newCurrent,
      nextItems: this.state.nextItems.slice(1)
    })
  }

  movePreviousItem() {

    if (this.state.previousItems.length === 0) {
      return;
    }

    var newNextItems = [this.state.currentItem].concat(this.state.nextItems);
    var previousItems = this.state.previousItems;
    var currentItem = previousItems.pop();
    this.setState({
      nextItems: newNextItems,
      currentItem,
      previousItems: previousItems,
    })
  }

  openItem(){
    window.open(this.state.currentItem.url, '_blank');
  }

  render() {
    return (<div className="feeds">
      <HotKey
      keys={['j']}
      onKeysCoincide={this.moveNextItem}
      />
      <HotKey
      keys={['k']}
      onKeysCoincide={this.movePreviousItem}
      />
      <HotKey
      keys={['o']}
      onKeysCoincide={this.openItem}
      />
      <FeedDisplay
      feeds={this.state.feeds}
      changeFeed={this.changeSelectedFeed}
      selectedId={this.state.selectedFeedId}
      />
       <FeedItems
      previousItems={this.state.previousItems}
      nextItems={this.state.nextItems}
      currentItem={this.state.currentItem}
      />
    </div>);
  }
}

module.exports = Feeds;
