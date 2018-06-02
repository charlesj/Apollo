import React from 'react'
import ClassNames from 'classnames'
import PropTypes from 'prop-types'
import { Container, } from '../_controls'

function FeedDisplay(props) {
  const { feeds, selected, changeFeed, } = props
  return (
    <Container width={300}>
      <div
        className={ClassNames({
          feed: true,
          selectedFeed: selected === null,
        })}
        onClick={() => changeFeed(null)}
      >
        All Items ({props.feeds.reduce((p, n) => {
          return p + n.unread_count
        }, 0)})
      </div>
      {feeds.map(feed => {
        return (
          <div
            className={ClassNames({
              feed: true,
              selectedFeed: selected && selected.id === feed.id,
            })}
            key={feed.id}
            onClick={() => changeFeed(feed)}
          >
            {feed.name} ({feed.unread_count})
          </div>
        )
      })}
    </Container>
  )
}

FeedDisplay.propTypes = {
  feeds: PropTypes.array.isRequired,
  selected: PropTypes.object,
  changeFeed: PropTypes.func.isRequired,
}


export default FeedDisplay