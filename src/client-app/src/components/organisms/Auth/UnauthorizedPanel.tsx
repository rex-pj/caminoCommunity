import * as React from "react";
import AuthBanner from "./AuthBanner";

const UnauthorizedPanel = () => {
  return (
    <div className="row g-0">
      <div className="col col-12">
        <AuthBanner
          imageUrl={""}
          title={`Bạn không thể truy cập trang này`}
          instruction="Có thể trang bị lỗi hoặc bạn chưa được cấp quyền truy cập"
        />
      </div>
    </div>
  );
};

export default UnauthorizedPanel;
