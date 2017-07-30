import _ from 'lodash';
import React from 'react';
import PropTypes from 'prop-types';
import { InputGroup, EditableText } from "@blueprintjs/core";

class EntryInput extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      note: '',
      newTag: '',
      tags: []
    }

    this.handleChange = this.handleChange.bind(this);
    this.handleTagChange = this.handleTagChange.bind(this);
    this.handleTagKeyPress = this.handleTagKeyPress.bind(this);
    this.addTag = this.addTag.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.removeTag = this.removeTag.bind(this);
  }

  handleChange(change) {
    this.setState({
      note: change
    });
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

  handleSubmit(event) {
    event.preventDefault();

    this.props.onSubmit(this.state.note, this.state.tags);
    this.setState({
      note: '',
      tags: []
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

  render() {
    return (<div>
        <div className="pt-card">
      <div className="pt-form-group">
        <EditableText

      maxLines={12}
      minLines={7}
      multiline
      placeholder="Add note"
      selectAllOnFocus={false}
      confirmOnEnterKey={false}
      value={this.state.note}
      onChange={this.handleChange}
      />
      </div>
      <div className="pt-form-group">
        <InputGroup
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
        <button type='submit' className='pt-button pt-intent-primary pt-icon-add buttonSpace' onClick={this.handleSubmit}>Save Note</button>

  </div>);
  }
}

EntryInput.propTypes = {
  onSubmit: PropTypes.func.isRequired
}

module.exports = EntryInput;
