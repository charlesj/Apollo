import moment from 'moment';
import React from 'react';
import FontAwesome from 'react-fontawesome';
import { InputGroup, Intent } from "@blueprintjs/core";
import QueueService from './QueueService';
import { Notifier } from '../../services/notifier';
import MarkdownRenderer from 'react-markdown-renderer';

class Queue extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      queueItems: [],
      newItemTitle: '',
      newItemLink: '',
      newItemDescription: ''
    };

    this.addItem = this.addItem.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.loadItems = this.loadItems.bind(this);
    this.markItemCompleted = this.markItemCompleted.bind(this);
  }

  componentDidMount() {
    this.loadItems();
  }

  loadItems() {
    QueueService.getQueueItems().then((data) => {
      this.setState({
        queueItems: data
      });
    });
  }

  markItemCompleted(item) {
    item.completed_at = new Date();
    QueueService.updateItem(item).then(() => {
      Notifier.show({
        intent: Intent.SUCCESS,
        message: "Marked item as complete",
      });
      this.loadItems();
    });
  }

  addItem() {
    var newItem = {
      title: this.state.newItemTitle,
      link: this.state.newItemLink,
      description: this.state.newItemDescription,
      completed_at: null
    }
    QueueService.addItem(newItem).then(() => {
      Notifier.show({
        intent: Intent.SUCCESS,
        message: "Successfully added queue item",
      });
      this.setState({
        newItemTitle: '',
        newItemLink: '',
        newItemDescription: ''
      })
      this.loadItems();
    });
  }

  handleChange(e) {
    this.setState({
      [e.target.name]: e.target.value
    });
  }

  render() {
    return (<div className="queuePanel">
      <h2>Queue</h2>
      <div className="pt-card queueAddItem">
        <InputGroup id="itemTitle" placeholder="title" onChange={this.handleChange} name="newItemTitle" value={this.state.newItemTitle} />
        <InputGroup id="itemLink" placeholder="link" onChange={this.handleChange} name="newItemLink" value={this.state.newItemLink} />
        <div><textarea className="pt-input" placeholder="description" onChange={this.handleChange} name="newItemDescription" value={this.state.newItemDescription} /></div>
        <button className='pt-button pt-intent-primary pt-icon-add buttonSpace' onClick={this.addItem}>Add Item</button>
      </div>


      {this.state.queueItems.map((item) => {
        var createTime = moment(item.created_at);
        return (
          <div className="queueList" key={item.id}>
              <div className="pt-card queueItem">
                <div className="queueHeader">
                  <span className="queueAdded">{createTime.calendar()}</span><a href={item.link}>{item.title}</a>
                </div>
                <div className="queueDescription">
                  <MarkdownRenderer markdown={item.description} />
                </div>
                <div className="queueFooter">
                  <button className="textButton green" onClick={this.markItemCompleted.bind(null, item)}><FontAwesome name='check-circle-o'  /> Mark Completed</button>
                </div>
              </div>
            </div>
          );
      })}



    </div>)
  }
}

module.exports = Queue;
