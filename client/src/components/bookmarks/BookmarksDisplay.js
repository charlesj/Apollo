var React = require('react');
var PropTypes = require('prop-types');
var moment = require('moment');

var apolloServer = require('../../services/apollo-server');

function SingleBookmark(props) {
  var createTime = moment(props.createdAt);
  return (<div className="bookmark"> { createTime.calendar() } - 
              <a href={props.link}>{props.title}</a>
            { props.description && <div className="description">{props.description}</div>}
            <div className="tags">
                { props.tags && props.tags.map((t, i) => {
      return (<span key={i} className="pt-tag">{t}</span>)
    })}
            </div>
        </div>)
}

SingleBookmark.propTypes = {
  createdAt: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  link: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired
}

class BookmarksDisplay extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      bookmarks: [],
      total: 0
    }

    this.loadBookmarks = this.loadBookmarks.bind(this);
  }

  componentDidMount() {
    this.loadBookmarks();
  }

  loadBookmarks() {
    var start = this.state.bookmarks.length;
    var currentBookmarks = this.state.bookmarks;
    apolloServer.invoke('getBookmarks', {
      start: start
    })
      .then(data => {
        this.setState({
          bookmarks: currentBookmarks.concat(data.bookmarks),
          total: data.total
        });
      });
  }

  render() {
    return (<div>
        Total Bookmarks: {this.state.total}
        {this.state.bookmarks.map(b => {
        return (<SingleBookmark
          createdAt={b.created_at}
          title={b.title}
          link={b.link}
          description={b.description}
          key={b.id}
          id={b.id}
          tags={b.tags}
          />)
      })}

        <button className="pt-button pt-intent-success buttonSpace" onClick={this.loadBookmarks}>Load More</button>
    </div>);
  }
}

module.exports = BookmarksDisplay;
