import React from "react";
import { connect } from "react-redux";
import { Shortcuts } from "react-shortcuts";
import { feedsActions } from "../../redux/actions";
import { feedsSelectors } from "../../redux/selectors";
import { shortcuts } from "../keymap";
import { FlexRow, FlexContainer } from "../_controls";
import FeedItems from "./FeedItems";
import FeedDisplay from "./FeedDisplay";
import "./feeds.css";

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

  handleShortcuts(action, event) {
    const { setCurrentItem, nextItem, previousItem } = this.props;

    switch (action) {
      case shortcuts.feeds.moveNext:
        setCurrentItem(nextItem);
        break;
      case shortcuts.feeds.movePrevious:
        setCurrentItem(previousItem);
        break;
      case shortcuts.feeds.openLink:
        this.openItem();
        break;
      default:
        console.warn("unknown shortcut");
    }
  }

  render() {
    const {
      next,
      previous,
      current,
      selectFeed,
      feeds,
      currentFeed
    } = this.props;
    return (
      <Shortcuts name="FEEDS" handler={(a, e) => this.handleShortcuts(a, e)}>
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
      </Shortcuts>
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
