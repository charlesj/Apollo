import React, { Component } from "react";
import { connect } from "react-redux";
import { userSettingsActions } from "../../redux/actions";
import { userSettingsSelectors } from "../../redux/selectors";
import {
  Container,
  AddButton,
  FlexRow,
  EditButton,
  FlexContainer
} from "../_controls";
import UserSettingsForm from "./UserSettingsForm";

import "./UserSettings.css";

class UserSettings extends Component {
  componentDidMount() {
    this.props.load();
  }

  handleSubmit(setting) {
    this.props.save(setting.name, setting.value);
    this.props.select(null);
  }

  render() {
    const { settings, selectedSetting, select } = this.props;
    return (
      <FlexRow>
        <FlexContainer>
          <Container>
            <table className="userSettingsTable">
              <thead>
                <tr>
                  <th>id</th>
                  <th>name</th>
                  <th>value</th>
                  <th>created</th>
                  <th>updated</th>
                  <th />
                </tr>
              </thead>
              <tbody>
                {settings.map(setting => {
                  return (
                    <tr key={setting.id}>
                      <td>{setting.id}</td>
                      <td>{setting.name}</td>
                      <td>{setting.value}</td>
                      <td>{setting.createdAtDisplay}</td>
                      <td>{setting.updatedAtDisplay}</td>
                      <td>
                        <EditButton onClick={() => select(setting)} small />
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
            <AddButton noun="User Setting" onClick={() => select({})} />
          </Container>
        </FlexContainer>
        {selectedSetting && (
          <FlexContainer>
            <Container>
              <UserSettingsForm
                setting={selectedSetting}
                onCancel={() => select(null)}
                onSubmit={values => this.handleSubmit(values)}
              />
            </Container>
          </FlexContainer>
        )}
      </FlexRow>
    );
  }
}

function mapStateToProps(state, props) {
  return {
    settings: userSettingsSelectors.all(state),
    selectedSetting: userSettingsSelectors.selected(state)
  };
}

function mapDispatchToProps(dispatch, props) {
  return {
    load: () => dispatch(userSettingsActions.load()),
    save: (name, value) => dispatch(userSettingsActions.save(name, value)),
    select: setting => dispatch(userSettingsActions.actions.select(setting))
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(UserSettings);
