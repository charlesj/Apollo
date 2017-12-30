import React from "react";
import moment from "moment";

class FeedItems extends React.Component {
  render() {
    if (this.props.currentItem === null) {
      return <div>No Unread Items</div>;
    }
    return (
      <div className="feedItemsContainer">
        {this.props.previousItems.map(i => {
          var displayDate = moment(i.published_at);
          return (
            <div className="previousFeedItem" key={i.id}>
              {i.title} - {i.feed_name}
              <span className="feedPubDate">{displayDate.calendar()}</span>
            </div>
          );
        })}

        <div className="currentFeedItem">
          <div className="feedItemTitle">
            <a href={this.props.currentItem.url}>
              {this.props.currentItem.title}
            </a>{" "}
            - {this.props.currentItem.feed_name}
            <span className="feedPubDate">
              {moment(this.props.currentItem.published_at).calendar()}
            </span>
          </div>
          <div className="feedItemBody">
            <div
              dangerouslySetInnerHTML={{
                __html: this.props.currentItem.body
              }}
            />
          </div>
        </div>

        {this.props.nextItems.map(i => {
          var displayDate = moment(i.published_at);
          return (
            <div className="nextFeedItem" key={i.id}>
              {i.title} - {i.feed_name}
              <span className="feedPubDate">{displayDate.calendar()}</span>
            </div>
          );
        })}
      </div>
    );
  }
}

export default FeedItems;
