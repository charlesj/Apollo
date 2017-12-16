import React from 'react';

import apolloServer from '../services/apollo-server';

class Summary extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      summaries: []
    };

    this.loadSummaries = this.loadSummaries.bind(this);
  }

  componentDidMount() {
    this.loadSummaries();
  }

  loadSummaries() {
    apolloServer.invoke("getSummaries", {}).then(data => {
      this.setState({
        summaries: data
      });
    });
  }

  render() {
    return (
      <div className="summaryContainer">
          { this.state.summaries.map((summary) => {
        return (<div key={summary.id} className="summary">
                  <div className="summaryAmount">{summary.amount}</div>
                  <div className="summaryLabel">{summary.label}</div>
              </div>)
      })}
      </div>
    )
  }
}

module.exports = Summary;
