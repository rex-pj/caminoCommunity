import React from "react";
import PromptLayout from "./PromptLayout";
import AuthBanner from "../../../components/organisms/Banner/AuthBanner";
import { getLocalStorageByKey } from "../../../services/StorageService";
import { AUTH_LOGIN_KEY } from "../../../utils/AppSettings";

function AuthLayout({ component: Component, ...rest }) {
  const isLogin = getLocalStorageByKey(AUTH_LOGIN_KEY);
  return (
    <PromptLayout
      {...rest}
      component={matchProps =>
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
    />
  );
}

export default AuthLayout;
