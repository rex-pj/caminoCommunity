import React from "react";
import { PageInfo } from "../../../utils/Constants";
import AuthBanner from "../Banner/AuthBanner";
import logoUrl from "../../../assets/images/logo.png";

const LogoutPanel = () => {
  return (
    <div className="row g-0">
      <div className="col col-12">
        <AuthBanner
          imageUrl={logoUrl}
          title={`Logout successfully ${PageInfo.BrandName}`}
          instruction="See you again"
        />
      </div>
    </div>
  );
};

export default LogoutPanel;
