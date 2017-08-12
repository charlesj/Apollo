import React from 'react';

class FeedItems extends React.Component {

  render() {
    console.log("feed item props", this.props);
    if (this.props.currentItem === null) {
      return (<div>No Unread Items</div>)
    }
    return (<div className='feedItemsContainer'>
      { this.props.previousItems.map(i => {
        return (<div className='previousFeedItem' key={i.id}>
          {i.title}
        </div>)
      })}

      <div className='currentFeedItem'>
        <div className='feedItemTitle'>{this.props.currentItem.title}</div>
        <div className='feedItemBody'>{this.props.currentItem.body}</div>
      </div>

      { this.props.nextItems.map(i => {
        return (<div className='nextFeedItem' key={i.id}>
          {i.title}
        </div>)
      })}
      </div>)
  }
}

module.exports = FeedItems;
