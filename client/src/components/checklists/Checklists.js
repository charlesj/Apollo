import React from 'react';
import _ from 'lodash';
import apolloServer from '../../services/apollo-server.js';
//import FontAwesome from 'react-fontawesome';
import ChecklistListing from './ChecklistListing';
import ChecklistDisplay from './ChecklistDisplay';
import CompleteChecklist from './CompleteChecklist';
import ChecklistCompletionLog from './ChecklistCompletionLog';
import CompletedChecklistDisplay from './CompletedChecklistDisplay';

class Checklists extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      checklists: [],
      selectedChecklist: null,
      selectedCompletedChecklist: null
    };

    this.deleteChecklist = this.deleteChecklist.bind(this);
    this.loadChecklists = this.loadChecklists.bind(this);
    this.newChecklist = this.newChecklist.bind(this);
    this.selectChecklist = this.selectChecklist.bind(this);
    this.upsertChecklist = this.upsertChecklist.bind(this);
    this.selectChecklistCompletion = this.selectChecklistCompletion.bind(this);
  }

  componentDidMount() {
    this.loadChecklists();
  }

  loadChecklists() {
    return apolloServer.invoke("GetChecklists", {}).then((checklists) => {
      this.setState({
        checklists
      });
    });
  }

  selectChecklist(checklist) {
    this.setState({
      selectedChecklist: checklist
    });
  }

  newChecklist(type) {
    var checklist = {
      name: "new checklist",
      type: type,
      description: "this is a new checklist"
    };

    return this.upsertChecklist(checklist);
  }

  upsertChecklist(checklist) {
    return apolloServer.invoke("upsertChecklist", {
      checklist
    }).then(() => {
      return this.loadChecklists();
    });
  }

  deleteChecklist(id) {
    return apolloServer.invoke("deleteChecklist", {
      id
    }).then(() => {
      return this.loadChecklists();
    });
  }

  selectChecklistCompletion(id) {
    this.setState({
      selectedCompletedChecklist: id
    });
  }

  render() {
    var checklistGroups = _.groupBy(this.state.checklists, 'type');
    return (<div className="checklistsPage"><div className="checklistsContainer">
      <div className="checklistsSelector">
      { _.map(checklistGroups, (checklists, type) => {
        return (<div className="checklistGroup" key={type}>
          <div className="checklistGroupHeading">{type}</div>
          {checklists.map((c) => {
            return <ChecklistListing
              checklist={c}
              key={c.id}
              upsert={this.upsertChecklist}
              select={this.selectChecklist.bind(null, c)}
              delete={this.deleteChecklist.bind(null, c.id)}
              />
          })}
          <button className="textButton" onClick={this.newChecklist.bind(null, type)}>new</button>
        </div>)
      })}
      </div>
      { this.state.selectedChecklist && (<ChecklistDisplay checklist={this.state.selectedChecklist} />) }
      <CompleteChecklist />
    </div>
    <div className="checklistReportingContainer">
      <ChecklistCompletionLog selectCompletion={this.selectChecklistCompletion} />
      {this.state.selectedCompletedChecklist && (<CompletedChecklistDisplay completionId={this.state.selectedCompletedChecklist} />)}
    </div>
  </div>)
  }
}

module.exports = Checklists;
