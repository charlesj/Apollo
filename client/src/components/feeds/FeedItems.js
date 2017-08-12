import React from 'react';

class FeedItems extends React.Component {

  render() {
    if (this.props.currentItem === null) {
      return (<div>No Unread Items</div>)
    }
    return (<div className='feedItemsContainer'>
      { this.props.previousItems.map(i => {
        return (<div className='previousFeedItem' key={i.id}>
          {i.title} - {i.feed_name}
        </div>)
      })}

      <div className='currentFeedItem'>
        <div className='feedItemTitle'>{this.props.currentItem.title} - {this.props.currentItem.feed_name}</div>
        <div className='feedItemBody'>
          <div dangerouslySetInnerHTML={{__html: this.props.currentItem.body}} />
        </div>
      </div>

      { this.props.nextItems.map(i => {
        return (<div className='nextFeedItem' key={i.id}>
          {i.title} - {i.feed_name}
        </div>)
      })}
      </div>)
  }
}

module.exports = FeedItems;
