import React from 'react';
import _ from 'lodash';
import apolloServer from '../../services/apollo-server.js';
import FontAwesome from 'react-fontawesome';

const itemTypes = ['mandatory','recommended','optional'];

function ChecklistItemListingDisplay(props){
  return (<div className="checklistItem" key={props.item.id}>{props.item.name}
    <FontAwesome name='edit' style={{'float': 'right'}} onClick={props.toggle} />
  </div>)
}

class ChecklistItemListing extends React.Component {
  constructor(props){
    super(props);
    this.state = {
      editMode: false,
      name: props.item.name,
      type: props.item.type,
      description: props.item.description
    }

    this.toggleEdit = this.toggleEdit.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.upsertItem = this.upsertItem.bind(this);
  }

  toggleEdit(){
    this.setState({editMode: !this.state.editMode});
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  upsertItem(){
    this.toggleEdit();
    var updated = this.props.item;
    updated.name = this.state.name;
    updated.type = this.state.type;
    updated.description = this.state.description;
    return this.props.upsert(updated);
  }

  render(){
    if(!this.state.editMode){
      return <ChecklistItemListingDisplay item={this.props.item} toggle={this.toggleEdit} />
    }

    return (<div className="checklistItem">
        <input id="name" placeholder="name" onChange={this.handleChange} name="name" value={this.state.name} /><br />
        <select name="type" value={this.state.type} onChange={this.handleChange} >
          {itemTypes.map(ct => {
            return <option value={ct} key={ct}>{ct}</option>
          })}
        </select><br />
        <textarea id="description" placeholder="description" onChange={this.handleChange} name="description" value={this.state.description}></textarea><br />
        <button className='pt-button pt-intent-primary' onClick={this.upsertItem}>Save</button>
        <button className='pt-button pt-intent-danger' onClick={this.props.delete}>Delete</button>
        <button className='textButton' onClick={this.toggleEdit}>Cancel</button>
    </div>);
  }
}

class ChecklistDisplay extends React.Component {
  constructor(props){
    super(props);

    this.state = {
      items: []
    };

    this.deleteItem = this.deleteItem.bind(this);
    this.loadItems = this.loadItems.bind(this);
    this.newItem = this.newItem.bind(this);
    this.upsertItem = this.upsertItem.bind(this);
  }

  componentDidMount(){
    this.loadItems(this.props.checklist.id);
  }

  componentWillReceiveProps(nextProps){
    this.loadItems(nextProps.checklist.id);
  }

  loadItems(checklistId){
    return apolloServer.invoke('GetChecklistItems', {id: checklistId})
    .then((items) => {
      this.setState({items});
    });
  }

  newItem(){
    var item = {
      checklist_id: this.props.checklist.id,
      name: "new item",
      type: "mandatory",
      description: ""
    };

    return this.upsertItem(item);
  }

  upsertItem(item){
    return apolloServer.invoke("UpsertChecklistItem", {item})
              .then(() => {this.loadItems(this.props.checklist.id)});
  }

  deleteItem(id){
    return apolloServer.invoke("DeleteChecklistItem", {id})
        .then(() => {this.loadItems(this.props.checklist.id)});
  }

  render(){
    return (<div className="checklistDisplay">
      <h2>{this.props.checklist.name}</h2>

      {this.state.items.map(item => {
        return (<ChecklistItemListing
          item={item}
          key={item.id}
          upsert={this.upsertItem}
          delete={this.deleteItem.bind(null, item.id)}
        />);
      })}
      <div className="checklistItem">
        <button className="textButton" onClick={this.newItem}>new</button>
      </div>
    </div>)
  }
}

module.exports = ChecklistDisplay;
