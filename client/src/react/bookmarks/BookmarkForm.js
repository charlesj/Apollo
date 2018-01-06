import React, { Component } from "react";
import { connect } from "react-redux";
import PropTypes from "prop-types";
import { Field, reduxForm } from "redux-form";
import { Button } from "../general";

class BookmarkForm extends Component {
  render() {
    const { handleSubmit, onCancel } = this.props;
    return (
      <div>
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
              component="input"
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
          <Button type="button" onClick={onCancel}>
            Cancel
          </Button>
          <Button type="submit" primary>
            Save
          </Button>
        </form>
      </div>
    );
  }
}

BookmarkForm.propTypes = {
  bookmark: PropTypes.object.isRequired,
  onSubmit: PropTypes.func.isRequired,
  onCancel: PropTypes.func.isRequired
};

BookmarkForm = reduxForm({
  form: "bookmarks",
  enableReinitialize: true
})(BookmarkForm);

function mapStateToProps(state, props) {
  const { bookmark } = props;

  return {
    initialValues: bookmark
  };
}

export default connect(mapStateToProps)(BookmarkForm);
