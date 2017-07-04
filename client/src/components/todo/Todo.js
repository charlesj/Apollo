import React from 'react';
import todoService from '../../services/todo-service';

class Todo extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      items: [],
      showCompleted: false,
      newItem: '',
    }

    this.toggleCompleted = this.toggleCompleted.bind(this);
    this.loadItems = this.loadItems.bind(this);
    this.toggleShowCompleted = this.toggleShowCompleted.bind(this);
    this.displayItems = this.displayItems.bind(this);
    this.shouldDisplay = this.shouldDisplay.bind(this);
    this.handleNewItemChange = this.handleNewItemChange.bind(this);
    this.handleNewItemKeyPress = this.handleNewItemKeyPress.bind(this);
  }

  componentDidMount() {
    this.loadItems();
  }

  loadItems() {
    todoService.getTodos().then(this.displayItems);
  }

  displayItems(data) {
    this.setState({
      items: data
    });
  }

  toggleCompleted(item) {
    todoService.toggleCompleted(item).then(() => {
      this.loadItems();
    });
  }

  toggleShowCompleted() {
    this.setState({
      showCompleted: !this.state.showCompleted
    });
  }

  shouldDisplay(item) {
    return (this.state.showCompleted && item.completed_at) ||
      !item.completed_at;
  }

  getItemClasses(item){
    if(item.completed_at){
      return "todoItemCompleted";
    }

    return "";
  }

  handleNewItemKeyPress(target) {
    if (target.charCode === 13) {
      todoService.addItem(this.state.newItem).then(() => {
        this.setState({
          newItem: ''
        });
        this.loadItems();
      });
    }
  }

  handleNewItemChange(event) {
    this.setState({
      newItem: event.target.value
    })
  }

  render() {
    return (<div>
      <div className="pt-card todoPanel">
        <h2>To Do</h2>
          {this.state.items.map((item, index) => {
            var checked = item.completed_at !== undefined && item.completed_at !== null;
            return (
              <div key={item.id}>
                { (this.shouldDisplay(item)) &&
                  (<div>
                    <input type="checkbox" checked={checked} onChange={this.toggleCompleted.bind(null, item)} value={checked} />
                    <span className={this.getItemClasses(item)}>{item.title}</span>
                  </div>)
                }
              </div>)
            })
          }
          <input type="text" value={this.state.newItem} onKeyPress={this.handleNewItemKeyPress} onChange={this.handleNewItemChange} />
          <button className="textButton green" onClick={this.toggleShowCompleted}>Toggle Completed</button>
        </div>
    </div>);
  }
}

module.exports = Todo;
