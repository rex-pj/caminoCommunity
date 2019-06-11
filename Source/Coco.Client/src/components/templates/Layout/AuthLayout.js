import React from "react";
import loadable from "@loadable/component";
import PromptLayout from "./PromptLayout";
import UserContext from "../../../utils/Context/UserContext";

const AuthBanner = loadable(() =>
  import("../../../components/organisms/Banner/AuthBanner")
);

function AuthLayout({ component: Component, ...rest }) {
  return (
    <PromptLayout
      {...rest}
      component={matchProps => (
        <UserContext.Consumer>
          {({ isLogin }) =>
            isLogin ? (
              <AuthBanner
                icon="exclamation-triangle"
                title="Bạn đã đăng nhập rồi"
                instruction="Hãy trở lại trang chủ để theo dõi nhiều nhà nông khác"
                actionUrl="/"
                actionText="Trở về trang chủ"
              />
            ) : (
              <Component {...matchProps} />
            )
          }
        </UserContext.Consumer>
      )}
    />
  );
}

export default AuthLayout;
