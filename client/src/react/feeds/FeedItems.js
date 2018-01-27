import React from "react";
import moment from "moment";

import { Container } from "../_controls";

function FeedItems(props) {
  const { previousItems, nextItems, currentItem } = props;

  console.log(props);

  return (
    <Container>
      {previousItems.map(i => {
        var displayDate = moment(i.published_at);
        return (
          <div className="previousFeedItem" key={i.id}>
            {i.title} - {i.feed_name}
            <span className="feedPubDate">{displayDate.calendar()}</span>
          </div>
        );
      })}
      {currentItem && (
        <div className="currentFeedItem">
          <div className="feedItemTitle">
            <a href={currentItem.url}>{currentItem.title}</a> -{" "}
            {currentItem.feed_name}
            <span className="feedPubDate">
              {moment(currentItem.published_at).calendar()}
            </span>
          </div>
          <div className="feedItemBody">
            <div
              dangerouslySetInnerHTML={{
                __html: currentItem.body
              }}
            />
          </div>
        </div>
      )}

      {nextItems.map(i => {
        var displayDate = moment(i.published_at);
        return (
          <div className="nextFeedItem" key={i.id}>
            {i.title} - {i.feed_name}
            <span className="feedPubDate">{displayDate.calendar()}</span>
          </div>
        );
      })}
    </Container>
  );
}

export default FeedItems;
