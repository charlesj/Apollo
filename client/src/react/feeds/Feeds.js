import React from "react";
import HotKey from "react-shortcut";
import { connect } from "react-redux";
import ClassNames from "classnames";
import { feedsActions } from "../../redux/actions";
import { feedsSelectors } from "../../redux/selectors";

import { Container, FlexRow, FlexContainer } from "../_controls";
import FeedItems from "./FeedItems";

import "./feeds.css";

function FeedDisplay(props) {
  const { feeds, selected, changeFeed } = props;
  return (
    <Container width={300}>
      <div
        className={ClassNames({
          feed: true,
          selectedFeed: selected === null
        })}
        onClick={() => changeFeed(null)}
      >
        All Items ({props.feeds.reduce((p, n) => {
          return p + n.unread_count;
        }, 0)})
      </div>
      {feeds.map(feed => {
        return (
          <div
            className={ClassNames({
              feed: true,
              selectedFeed: selected && selected.id === feed.id
            })}
            key={feed.id}
            onClick={() => changeFeed(feed)}
          >
            {feed.name} ({feed.unread_count})
          </div>
        );
      })}
    </Container>
  );
}

class Feeds extends React.Component {
  componentDidMount() {
    const { loadList, loadItems } = this.props;
    loadList();
    loadItems(-1);
  }

  openItem() {
    const { current } = this.props;
    if (current) {
      window.open(current.url, "_blank");
    }
  }

  render() {
    const {
      nextItem,
      next,
      previousItem,
      previous,
      current,
      setCurrentItem,
      selectFeed,
      feeds,
      currentFeed
    } = this.props;
    return (
      <FlexRow>
        <HotKey keys={["j"]} onKeysCoincide={() => setCurrentItem(nextItem)} />
        <HotKey
          keys={["k"]}
          onKeysCoincide={() => setCurrentItem(previousItem)}
        />
        <HotKey keys={["o"]} onKeysCoincide={() => this.openItem()} />
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
    );
  }
}

function mapStateToProps(state, props) {
  return {
    feeds: feedsSelectors.feeds(state),
    currentFeed: feedsSelectors.currentFeed(state),
    ...feedsSelectors.displayItems(state)
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    loadList: () => dispatch(feedsActions.loadList()),
    loadItems: feedId => dispatch(feedsActions.loadItems(feedId)),
    setCurrentItem: item => dispatch(feedsActions.setCurrentItem(item)),
    selectFeed: feed => dispatch(feedsActions.selectFeed(feed))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(Feeds);
