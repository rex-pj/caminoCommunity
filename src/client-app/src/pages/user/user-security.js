import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { SessionContext } from "../../store/context/session-context";
import PasswordUpdateForm from "../../components/organisms/Profile/PasswordUpdateForm";
import { useStore } from "../../store/hook-store";
import ConfirmToRedirectModal from "../../components/organisms/Modals/ConfirmToRedirectModal";
import AuthService from "../../services/authService";
import { setLogin, checkRemember } from "../../services/AuthLogic";

const UserSecurity = (props) => {
  const authService = new AuthService();
  const navigate = useNavigate();
  const [isFormEnabled, setFormEnabled] = useState(true);
  const dispatch = useStore(false)[1];
  const { canEdit } = props;
  const { relogin } = useContext(SessionContext);

  const onUpdateConfirmation = () => {
    dispatch("OPEN_MODAL", {
      data: {
        title: "You will need to log out and log in again",
        children:
          "To make sure all functions are working properly you need to log out and log in again",
        executeButtonName: "Ok",
      },
      execution: {
        onSucceed: onSucceed,
      },
      options: {
        isOpen: true,
        innerModal: ConfirmToRedirectModal,
        unableClose: true,
      },
    });
  };

  const onSucceed = () => {
    navigate("/auth/logout");
  };

  const onUpdatePassword = async (data) => {
    if (!canEdit) {
      return;
    }

    setFormEnabled(true);

    await authService
      .updatePassword(data)
      .then(async (response) => {
        const { data } = response;
        const { authenticationToken } = data;
        if (!authenticationToken) {
          setFormEnabled(true);
          return Promise.reject();
        }

        const isRemember = checkRemember();
        setLogin(data, isRemember);
        await relogin();

        onUpdateConfirmation();
        setFormEnabled(true);
        Promise.resolve(data);
      })
      .catch((error) => {
        setFormEnabled(true);
        Promise.reject(error);
      });
  };

  return (
    <PasswordUpdateForm
      onUpdate={(e) => onUpdatePassword(e)}
      isFormEnabled={isFormEnabled}
      canEdit={canEdit}
      showValidationError={props.showValidationError}
    />
  );
};

export default UserSecurity;
