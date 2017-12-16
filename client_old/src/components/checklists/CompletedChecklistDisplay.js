import React from 'react';
import moment from 'moment';
import FontAwesome from 'react-fontawesome';
import apolloServer from '../../services/apollo-server.js';

class CompletedChecklistDisplay extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      checklistInfo: null,
      items: null
    }
  }

  componentDidMount() {
    this.loadInformation(this.props.completionId);
  }

  componentWillReceiveProps(newProps) {
    this.loadInformation(newProps.completionId);
  }

  loadInformation(completed_checklist_id) {
    apolloServer.invoke('GetChecklistCompletion', {
      completed_checklist_id
    })
      .then(data => {
        this.setState({
          checklistInfo: data.checklistInfo,
          items: data.items
        });
      });
  }

  render() {
    if (!this.state.checklistInfo) {
      return (<div className="checklistCompletionDisplayContainer">loading...</div>)
    }

    const completed_at = moment(this.state.checklistInfo.completed_at);

    return (<div className="checklistCompletionDisplayContainer">
      <div className="compeletedChecklistHeader">
      {this.state.checklistInfo.name} - {completed_at.calendar()}
      </div>
      <p>{this.state.checklistInfo.notes}</p>
      { this.state.items.map(item => {
        return (<div className="completedChecklistItem" key={item.id}>
          {item.completed === 0 && (<FontAwesome name='circle-o' className='red' />) }
          {item.completed > 0 && (<FontAwesome name='check-circle-o' className='green' />)}
            {item.name}
        </div>)
      })}
    </div>)
  }
}

module.exports = CompletedChecklistDisplay;
