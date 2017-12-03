import React from 'react';
import moment from 'moment';
import apolloServer from '../../services/apollo-server.js';

class ChecklistCompletionLog extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      completions: []
    };
  }
  componentDidMount() {
    apolloServer.invoke("GetChecklistCompletionLog", {})
      .then(completions => {
        this.setState({
          completions
        });
      });
  }

  render() {
    return (<div className="completionLogContainer">
      <h1>Completion Log</h1>
      { this.state.completions.map(c => {
        let completed_at = moment(c.completed_at);
        return (<div className="completionLogEntry" key={c.completion_id}>
          {completed_at.calendar()}: <button className="textButton" onClick={this.props.selectCompletion.bind(null, c.completion_id)}>{c.name}</button>
        </div>)
      })}
    </div>)
  }
}

module.exports = ChecklistCompletionLog;
