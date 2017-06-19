var React = require('react');
import { Tab2, Tabs2 } from "@blueprintjs/core";

const BookmarksDisplay = require('./BookmarksDisplay');
const AddBookmark = require('./AddBookmark');

class Bookmarks extends React.Component {

  render() {
    return (<div>
        <Tabs2 id="BookmarkTabs" >
                <Tab2 id="bookmarks" title="Bookmarks" panel={<BookmarksDisplay />} />
                <Tab2 id="add_bookmark" title="Add bookmark" panel={<AddBookmark />} />
                <Tabs2.Expander />
            </Tabs2></div>);
  }
}

module.exports = Bookmarks;
