import React from 'react';
import _ from 'lodash';
import apolloServer from '../../services/apollo-server.js';
import FontAwesome from 'react-fontawesome';

var checklistTypes = ['daily', 'weekly', 'monthly', 'ad-hoc'];

function CheckListListingDisplay(props){
   return (<div className="checklistListingDisplay">
     <span onClick={props.select}>{props.checklist.name}</span>
     <FontAwesome name='edit' style={{'float': 'right'}} onClick={props.toggle} />
   </div>)
}

class ChecklistListing extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      edit: false,
      name: props.checklist.name,
      type: props.checklist.type,
      description: props.checklist.description
    }
    this.toggleMode = this.toggleMode.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.upsertChecklist = this.upsertChecklist.bind(this);
  }


  toggleMode(){
    this.setState({edit: !this.state.edit});
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  upsertChecklist(){
    this.toggleMode();
    var updated = this.props.checklist;
    updated.name = this.state.name;
    updated.type = this.state.type;
    updated.description = this.state.description;
    this.props.upsert(updated);
  }

  render(){
    if(!this.state.edit){
      return <CheckListListingDisplay checklist={this.props.checklist} select={this.props.select} toggle={this.toggleMode}/>
    }

    return (<div className="checklistListing">
        <input id="name" placeholder="name" onChange={this.handleChange} name="name" value={this.state.name} />
        <select name="type" value={this.state.type} onChange={this.handleChange} >
          {checklistTypes.map(ct => {
            return <option value={ct} key={ct}>{ct}</option>
          })}
        </select>
        <textarea id="description" placeholder="description" onChange={this.handleChange} name="description" value={this.state.description}></textarea>
        <button className='pt-button pt-intent-primary' onClick={this.upsertChecklist}>Save</button>
        <button className='pt-button pt-intent-danger' onClick={this.props.delete}>Delete</button>
        <button className='textButton' onClick={this.toggleMode}>Cancel</button>
    </div>)
  }
}

class Checklists extends React.Component {
  constructor(props){
    super(props);

    this.state = {
      checklists: [],
      selectedChecklist: null
    };

    this.deleteChecklist = this.deleteChecklist.bind(this);
    this.loadChecklists = this.loadChecklists.bind(this);
    this.newChecklist = this.newChecklist.bind(this);
    this.selectChecklist = this.selectChecklist.bind(this);
    this.upsertChecklist = this.upsertChecklist.bind(this);
  }

  componentDidMount(){
    this.loadChecklists();
  }

  loadChecklists(){
    return apolloServer.invoke("GetChecklists", {}).then((checklists) => {
      this.setState({
        checklists
      });
    });
  }

  selectChecklist(checklist){
    this.setState({selectedChecklist: checklist});
  }

  newChecklist(type){
    var checklist={
      name: "new checklist",
      type: type,
      description: "this is a new checklist"
    };

    return this.upsertChecklist(checklist);
  }

  upsertChecklist(checklist){
    return apolloServer.invoke("upsertChecklist", {checklist}).then(() => {
      return this.loadChecklists();
    });
  }

  deleteChecklist(id){
    return apolloServer.invoke("deleteChecklist", {id}).then(() => {
      return this.loadChecklists();
    });
  }

  render(){
    var checklistGroups = _.groupBy(this.state.checklists, 'type');
    return (<div className="checklistsContainer">
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
      { this.state.selectedChecklist && (<div className="checklistDisplay">{this.state.selectedChecklist.name}</div>) }
    </div>)
  }
}

module.exports = Checklists;
