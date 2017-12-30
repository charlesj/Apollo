import React from 'react';
import _ from 'lodash';

class AddBookmark extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      showSuccess: false,
      showError: false,
      title: '',
      link: '',
      description: '',
      tags: [],
      newTag: ''
    }

    this.handleTagChange = this.handleTagChange.bind(this);
    this.handleTagKeyPress = this.handleTagKeyPress.bind(this);
    this.addTag = this.addTag.bind(this);
    this.removeTag = this.removeTag.bind(this);
    this.updateTitle = this.updateTitle.bind(this);
    this.updateLink = this.updateLink.bind(this);
    this.updateDescription = this.updateDescription.bind(this);
    this.addBookmark = this.addBookmark.bind(this);
  }

  handleTagChange(event) {
    this.setState({
      newTag: event.target.value
    });
  }

  handleTagKeyPress(target) {
    if (target.charCode === 13) {
      this.addTag();
    }
  }

  updateTitle(event) {
    this.setState({
      title: event.target.value
    });
  }

  updateLink(event) {
    this.setState({
      link: event.target.value
    });
  }

  updateDescription(event) {
    this.setState({
      description: event.target.value
    });
  }

  addTag() {
    var currentTags = this.state.tags;
    currentTags.push(this.state.newTag);
    this.setState({
      newTag: '',
      tags: currentTags
    });
  }

  removeTag(tag) {
    var currentTags = this.state.tags;
    _.remove(currentTags, t => {
      return t === tag
    });
    this.setState({
      newTag: '',
      tags: currentTags
    });
  }

  addBookmark() {
    this.props.addBookmark(
      this.state.title,
      this.state.link,
      this.state.description,
      this.state.tags)
      .then(data => {
        this.setState({
          showError: false,
          showSuccess: true,
          title: '',
          link: '',
          description: '',
          tags: []
        });
      }).catch(err => {
      this.setState({
        showError: true,
        showSuccess: false
      });
    })
  }

  render() {
    return (<div>
      <div>
      {this.state.showSuccess && <p className="pt-callout pt-intent-success">Successfully added bookmark</p>}
      {this.state.showError && <p className="pt-callout pt-intent-danger">Could not add bookmark</p> }

          <div className="pt-card">
        <div className="pt-form-group">
          <input
      type="text"
      className="pt-input"
      id="title"
      placeholder="Title"
      value={this.state.title}
      onChange={this.updateTitle} />
        </div>
        <div className="pt-form-group">
          <input
      type="text"
      className="pt-input"
      id="link"
      placeholder="Url"
      value={this.state.link}
      onChange={this.updateLink} />
        </div>
        <div className="pt-form-group">
          <input
      type="text"
      className="pt-input"
      id="description"
      placeholder="Description"
      value={this.state.description}
      onChange={this.updateDescription} />
        </div>
        <div className="pt-form-group">
          <input
      leftIconName="tag"
      onChange={this.handleTagChange}
      placeholder="Add tags"
      value={this.state.newTag}
      onKeyPress={this.handleTagKeyPress}
      />
        </div>
        <div>
        {this.state.tags.map((tag) => {
        return (<span className='pt-tag pt-tag-removable' key={tag}>
                  {tag}
                  <button className='pt-tag-remove' type='button' onClick={this.removeTag.bind(null, tag)}></button></span>)
      })}
        </div>
        </div>
        <button type="submit" className="pt-button pt-intent-primary pt-icon-add buttonSpace" onClick={this.addBookmark}>Add Bookmark</button>

      </div>
    </div>);
  }
}

export default AddBookmark;
