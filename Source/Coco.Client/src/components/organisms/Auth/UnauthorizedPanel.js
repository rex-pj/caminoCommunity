import React from "react";
import AuthBanner from "../Banner/AuthBanner";

export default function() {
  return (
    <div className="row no-gutters">
      <div className="col col-12">
        <AuthBanner
          imageUrl={`${process.env.PUBLIC_URL}/images/logo.png`}
          title={`Bạn không thể truy cập trang này`}
          instruction="Có thể trang bị lỗi hoặc bạn chưa được cấp quyền truy cập"
        />
      </div>
    </div>
  );
};
