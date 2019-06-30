import React, { Component, } from 'react'
import { connect, } from 'react-redux'
import PropTypes from 'prop-types'

import { Field, reduxForm, } from 'redux-form'
import { CancelButton, SaveButton, Container, } from '../_controls'

import './BookmarkForm.css'

class BookmarkForm extends Component {
  render() {
    const { handleSubmit, onCancel, } = this.props
    return (
      <Container className="bookmarkFormContainer" width={400}>
        <form onSubmit={handleSubmit}>
          <div>
            <Field
              name="title"
              component="input"
              type="text"
              placeholder="Title"
            />
          </div>
          <div>
            <Field
              name="link"
              component="input"
              type="text"
              placeholder="Link"
            />
          </div>
          <div>
            <Field
              name="description"
              component="textarea"
              type="text"
              placeholder="Description"
            />
          </div>
          <div>
            <Field
              name="unifiedTags"
              component="input"
              type="text"
              placeholder="Tags"
            />
          </div>
          <CancelButton type="button" onClick={onCancel} />
          <SaveButton type="submit" primary />
        </form>
      </Container>
    )
  }
}

BookmarkForm.propTypes = {
  bookmark: PropTypes.object.isRequired,
  handleSubmit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired,
}

BookmarkForm = reduxForm({ // eslint-disable-line no-class-assign
  form: 'bookmarks',
  enableReinitialize: true,
})(BookmarkForm)

function mapStateToProps(state, props) {
  const { bookmark, } = props

  return {
    initialValues: bookmark,
  }
}

export default connect(mapStateToProps)(BookmarkForm)
