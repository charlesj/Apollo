import React from 'react';
import _ from 'lodash';
import FontAwesome from 'react-fontawesome';

var checklistTypes = ['daily', 'weekly', 'monthly', 'ad-hoc'];

function CheckListListingDisplay(props) {
  return (<div className="checklistListingDisplay">
     <span onClick={props.select}>{props.checklist.name}</span>
     <FontAwesome name='edit' style={{
      'float': 'right'
    }} onClick={props.toggle} />
   </div>)
}

class ChecklistListing extends React.Component {
  constructor(props) {
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


  toggleMode() {
    this.setState({
      edit: !this.state.edit
    });
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  upsertChecklist() {
    this.toggleMode();
    var updated = this.props.checklist;
    updated.name = this.state.name;
    updated.type = this.state.type;
    updated.description = this.state.description;
    return this.props.upsert(updated);
  }

  render() {
    if (!this.state.edit) {
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

module.exports = ChecklistListing;
