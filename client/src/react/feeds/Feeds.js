import React from 'react'
import { connect, } from 'react-redux'
import { HotKeys, } from 'react-hotkeys'
import PropTypes from 'prop-types'
import { feedsActions, } from '../../redux/actions'
import { feedsSelectors, } from '../../redux/selectors'
import { FlexRow, FlexContainer, } from '../_controls'
import FeedItems from './FeedItems'
import FeedDisplay from './FeedDisplay'
import './feeds.css'

const shortcuts = {
  moveNext: 'moveNext',
  movePrevious: 'movePrevious',
  openLink: 'openLink',
}

const feedsKeyMap = {
  [shortcuts.moveNext]: 'j',
  [shortcuts.movePrevious]: 'k',
  [shortcuts.openLink]: 'o',
}

class Feeds extends React.Component {
  componentDidMount() {
    const { loadList, loadItems, } = this.props
    loadList()
    loadItems(-1)
  }

  openItem() {
    const { current, } = this.props
    if (current) {
      window.open(current.url, '_blank')
    }
  }

  render() {
    const {
      next,
      previous,
      current,
      selectFeed,
      feeds,
      currentFeed,
      setCurrentItem,
      nextItem,
      previousItem,
    } = this.props

    const shortcutHandlers = {
      [shortcuts.moveNext]: () => {
        document.body.scrollTop = document.documentElement.scrollTop = 0
        nextItem && setCurrentItem(nextItem)
      },
      [shortcuts.movePrevious]: () => {
        document.body.scrollTop = document.documentElement.scrollTop = 0
        previousItem && setCurrentItem(previousItem)
      },
      [shortcuts.openLink]: () => this.openItem(),
    }

    return (
      <HotKeys keyMap={feedsKeyMap} handlers={shortcutHandlers}>
        <FlexRow>
          <FlexContainer>
            <FeedDisplay
              feeds={feeds}
              changeFeed={feed => selectFeed(feed)}
              selected={currentFeed}
            />
          </FlexContainer>
          <FlexContainer grow>
            <FeedItems
              previousItems={previous}
              nextItems={next}
              currentItem={current}
            />
          </FlexContainer>
        </FlexRow>
      </HotKeys>
    )
  }
}

Feeds.propTypes = {
  next: PropTypes.array.isRequired,
  previous: PropTypes.array.isRequired,
  current: PropTypes.object,
  selectFeed: PropTypes.func.isRequired,
  feeds: PropTypes.array.isRequired,
  currentFeed: PropTypes.object,
  setCurrentItem: PropTypes.func.isRequired,
  nextItem: PropTypes.object,
  previousItem: PropTypes.object,
  loadList: PropTypes.func.isRequired,
  loadItems: PropTypes.func.isRequired,
}

function mapStateToProps(state) {
  return {
    feeds: feedsSelectors.feeds(state),
    currentFeed: feedsSelectors.currentFeed(state),
    ...feedsSelectors.displayItems(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    loadList: () => dispatch(feedsActions.loadList()),
    loadItems: feedId => dispatch(feedsActions.loadItems(feedId)),
    setCurrentItem: item => dispatch(feedsActions.setCurrentItem(item)),
    selectFeed: feed => dispatch(feedsActions.selectFeed(feed)),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Feeds)
