var React = require('react');
var PropTypes = require('prop-types');
var moment = require('moment');

var apollo = require('../../services/apollo-server');

function EntryDisplay(props) {
    var createTime = moment(props.createdAt);
  return (<div className="note">

        <div className="createdTime">
        { createTime.format('Y-MM-DD HH:mm:SS') }
        </div>
        <div className="content">
        { props.note }
        </div>
    </div>)
}

EntryDisplay.propTypes = {
  createdAt: PropTypes.string.isRequired,
  note: PropTypes.string.isRequired,
  id: PropTypes.number.isRequired
}

class Journal extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      entries: []
    }

    this.loadEntries = this.loadEntries.bind(this);
  }

  componentDidMount() {
    this.loadEntries();
  }

  loadEntries() {
    this.setState(() => {
      return {
        entries: []
      }
    });

    apollo.invoke('GetAllJournalEntries', {})
      .then(data => {
        this.setState(() => {
          return {
            entries: data
          }
        })
      });
  }

  render() {
    return (
      <div>
                <h2>Journal</h2>
                    { this.state.entries.map(function(entry, index) {
        return (<EntryDisplay
          id={entry.id}
          key={entry.id}
          note={entry.note}
          createdAt={entry.created_at}
          />)
      }, this)}
            </div>
      );
  }
}


module.exports = Journal;
