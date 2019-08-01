import * as notifyActions from "./actions/notifyActions";
import * as modalActions from "./actions/modalActions";
import * as avatarActions from "./actions/avatarActions";

function raiseError(dispatch, title, description, url) {
  dispatch({
    type: notifyActions.NOTIFICATION,
    payload: {
      type: "error",
      title: title,
      description: description,
      url: url
    }
  });
}

function openModal(dispatch, e) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      ...e,
      isOpen: true
    }
  });
}

function closeModal(dispatch) {
  dispatch({
    type: modalActions.OPEN,
    payload: {
      isOpen: false
    }
  });
}

function avatarUploaded(dispatch, data) {
  dispatch({
    type: avatarActions.AVATAR_UPLOADED,
    payload: {
      ...data
    }
  });
}

function avatarDeleted(dispatch) {
  dispatch({
    type: avatarActions.AVATAR_DELETED
  });
}

function avatarReload(dispatch) {
  dispatch({
    type: avatarActions.AVATAR_RELOAD
  });
}

export {
  raiseError,
  openModal,
  closeModal,
  avatarUploaded,
  avatarDeleted,
  avatarReload
};
