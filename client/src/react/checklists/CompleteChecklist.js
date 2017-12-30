import React from 'react';
import { NotifySuccess } from '../../services/notifier';
import apolloServer from '../../services/apolloServer';
import { TextButton } from '../general';

class CompleteChecklist extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      active: false,
      checklists: [],
      selectedChecklistId: null,
      notes: ''
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleChecklistChange = this.handleChecklistChange.bind(this);
    this.activate = this.activate.bind(this);
    this.toggleActive = this.toggleActive.bind(this);
    this.toggleChecklistItemComplete = this.toggleChecklistItemComplete.bind(this);
    this.getChecklistItemClasses = this.getChecklistItemClasses.bind(this);
    this.saveCompletion = this.saveCompletion.bind(this);
  }

  toggleActive() {
    if (this.state.active) {
      this.setState({
        active: false
      });
    } else {
      this.activate();
    }
  }

  activate() {
    apolloServer.invoke('GetChecklists', {}).then(checklists => {
      apolloServer.invoke('getChecklistItems', {
        id: checklists[0].id
      })
        .then((checkListItems) => {
          this.setState({
            checklists,
            selectedChecklistId: checklists[0].id,
            checkListItems,
            active: true
          });
        })
    });
  }


  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  handleChecklistChange(e) {
    var selectedChecklistId = e.target.value;
    apolloServer.invoke('getChecklistItems', {
      id: selectedChecklistId
    })
      .then((checkListItems) => {
        this.setState({
          selectedChecklistId,
          checkListItems
        });
      })
  }

  toggleChecklistItemComplete(checklist_item_id) {
    var updated = this.state.checkListItems.map(i => {
      if (i.id !== checklist_item_id) {
        return i;
      }
      if (!i.completed || i.completed === 0) {
        i.completed = 1;
        return i;
      }
      i.completed = 0;
      return i;
    });

    this.setState({
      checklistItems: updated
    });
  }

  getChecklistItemClasses(item) {
    var classes = "checklistCompletionItemDisplay";
    switch (item.type) {
      case "mandatory":
        return classes + " checklistCompletionItemMandatory";
      case "recommended":
        return classes + " checklistCompletionItemRecommended";
      default:
        return classes + " checklistCompletionItemOptional";
    }
  }

  saveCompletion() {
    var payload = {
      checklist_id: this.state.selectedChecklistId,
      notes: this.state.notes,
      items: this.state.checkListItems.map(item => {
        return {
          checklist_item_id: item.id,
          completed: item.completed ? item.completed : 0
        }
      })
    };
    apolloServer.invoke("AddCompletedChecklist", payload).then(() => {
      NotifySuccess("Successfully save checklist.");
      this.setState({
        active: false
      });
    });
  }

  render() {
    if (!this.state.active) {
      return (<div className="checklistCompletionContainer"><button className="textButton" onClick={this.toggleActive}>Start a checklist</button></div>)
    }

    return (<div className="checklistCompletionContainer">
        <div className="checklistCompletionHeader">
          <select name="selectedChecklist" value={this.state.selectedChecklistId} onChange={this.handleChecklistChange} >
            {this.state.checklists.map(ct => {
        return <option value={ct.id} key={ct.id}>{ct.name}</option>
      })}
          </select>
          <TextButton onClick={this.toggleActive}>Cancel</TextButton>
          <TextButton onClick={this.saveCompletion}>Save</TextButton>
        </div>
        <div className="checklistCompletionItemList">
          {this.state.checkListItems.map((item) => {
        return (<div className={this.getChecklistItemClasses(item)} key={item.id}>
              <input type="checkbox"
          className="pt-large"
          checked={item.completed === 1}
          onChange={this.toggleChecklistItemComplete.bind(null, item.id)}
          /> {item.name} <em>{item.description}</em>
            </div>)
      })}
          <textarea name="notes" id="notes" onChange={this.handleChange} value={this.state.notes} placeholder="enter any notes here"/>
        </div>
    </div>)
  }
}

export default CompleteChecklist;
