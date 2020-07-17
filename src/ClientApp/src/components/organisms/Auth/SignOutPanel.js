import React from "react";
import { PageInfo } from "../../../utils/Constant";
import AuthBanner from "../Banner/AuthBanner";

export default () => {
  return (
    <div className="row no-gutters">
      <div className="col col-12">
        <AuthBanner
          imageUrl={`${process.env.PUBLIC_URL}/images/logo.png`}
          title={`Bạn vừa rời khỏi ${PageInfo.BrandName}`}
          instruction="Hẹn gặp lại bạn vào lần sau"
        />
      </div>
    </div>
  );
};
