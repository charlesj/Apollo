import React from 'react';
import { Tab2, Tabs2 } from "@blueprintjs/core";
import apolloServer from '../../services/apollo-server';
import BookmarksDisplay from './BookmarksDisplay';
import AddBookmark from './AddBookmark';

class Bookmarks extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      bookmarks: [],
      totalBookmarks: 0
    };

    this.loadBookmarks = this.loadBookmarks.bind(this);
    this.addBookmark = this.addBookmark.bind(this);
    this.refreshBookmarks = this.refreshBookmarks.bind(this);
  }

  loadBookmarks() {
    var start = this.state.bookmarks.length;
    var currentBookmarks = this.state.bookmarks;
    apolloServer.invoke('getBookmarks', {
      start: start
    }).then(data => {
      this.setState({
        bookmarks: currentBookmarks.concat(data.bookmarks),
        totalBookmarks: data.total
      });
    });
  }

  componentDidMount() {
    this.loadBookmarks();
  }

  addBookmark(title, link, description, tags) {
    return apolloServer.invoke('addBookmark', {
      title: title,
      link: link,
      description: description,
      tags: tags
    }).then(() => {
      this.refreshBookmarks();
    });
  }

  refreshBookmarks() {
    this.setState({
      bookmarks: []
    });
    this.loadBookmarks();
  }

  render() {
    return (<div>
        <Tabs2 id="BookmarkTabs" >
          <Tab2 id="bookmarks" title="Bookmarks" panel={
      <BookmarksDisplay
      bookmarks={this.state.bookmarks}
      totalBookmarks={this.state.totalBookmarks}
      loadBookmarks={this.loadBookmarks}
      refreshBookmarks={this.refreshBookmarks}
      />}
      />
          <Tab2 id="add_bookmark" title="Add bookmark" panel={
      <AddBookmark addBookmark={this.addBookmark} />
      } />
          <Tabs2.Expander />
        </Tabs2></div>);
  }
}

module.exports = Bookmarks;
