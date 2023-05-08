import * as React from "react";
import { useState } from "react";
import { Routes, Route } from "react-router-dom";

const UserAboutPage = React.lazy(
  () => import("../../../pages/user/user-about")
);
const UserUpdatePage = React.lazy(
  () => import("../../../pages/user/user-update")
);
const UserSecurityPage = React.lazy(
  () => import("../../../pages/user/user-security")
);
const UserArticlesPage = React.lazy(
  () => import("../../../pages/user/user-articles")
);
const UserProductsPage = React.lazy(
  () => import("../../../pages/user/user-products")
);
const UserFarmsPage = React.lazy(
  () => import("../../../pages/user/user-farms")
);
const UserFollowingsPage = React.lazy(
  () => import("../../../pages/user/user-followings")
);
const UserFeedsPage = React.lazy(
  () => import("../../../pages/user/user-feeds")
);

const ProfileInfo = React.lazy(() => import("./ProfileInfo"));

const Profile = (props) => {
  const { userId, userInfo, pageNumber } = props;
  const { canEdit } = userInfo;

  const [editorMode, setEditorMode] = useState("ARTICLE");
  const onToggleCreateMode = (name) => {
    setEditorMode(name);
  };

  return (
    <div className="row">
      <div className="col col-12 col-sm-12 col-md-12 col-lg-9 or-last order-lg-first">
        <Routes>
          <Route
            path="/about"
            element={
              <UserAboutPage
                userId={userId}
                canEdit={canEdit}
                pageNumber={pageNumber}
                editorMode={editorMode}
                onToggleCreateMode={onToggleCreateMode}
              />
            }
          />
          <Route
            path="/update"
            element={
              <UserUpdatePage
                userId={userId}
                canEdit={canEdit}
                pageNumber={pageNumber}
                editorMode={editorMode}
                onToggleCreateMode={onToggleCreateMode}
              />
            }
          />
          <Route
            path="/security"
            element={
              <UserSecurityPage
                userId={userId}
                canEdit={canEdit}
                pageNumber={pageNumber}
                editorMode={editorMode}
                onToggleCreateMode={onToggleCreateMode}
              />
            }
          />
          {["/articles", "/articles/page/:pageNumber"].map((route) => {
            return (
              <Route
                key={route}
                path={route}
                element={
                  <UserArticlesPage
                    userId={userId}
                    canEdit={canEdit}
                    pageNumber={pageNumber}
                    editorMode={editorMode}
                    onToggleCreateMode={onToggleCreateMode}
                  />
                }
              />
            );
          })}
          {["/products", "/products/page/:pageNumber"].map((route) => {
            return (
              <Route
                key={route}
                path={route}
                element={
                  <UserProductsPage
                    userId={userId}
                    canEdit={canEdit}
                    pageNumber={pageNumber}
                    editorMode={editorMode}
                    onToggleCreateMode={onToggleCreateMode}
                  />
                }
              />
            );
          })}
          {["/farms", "/farms/page/:pageNumber"].map((route) => {
            return (
              <Route
                key={route}
                path={route}
                element={
                  <UserFarmsPage
                    userId={userId}
                    canEdit={canEdit}
                    pageNumber={pageNumber}
                    editorMode={editorMode}
                    onToggleCreateMode={onToggleCreateMode}
                  />
                }
              />
            );
          })}
          {["/followings", "/followings/page/:pageNumber"].map((route) => {
            return (
              <Route
                key={route}
                path={route}
                element={
                  <UserFollowingsPage
                    userId={userId}
                    canEdit={canEdit}
                    pageNumber={pageNumber}
                    editorMode={editorMode}
                    onToggleCreateMode={onToggleCreateMode}
                  />
                }
              />
            );
          })}
          {["/*", "/feeds", "/feeds/page/:pageNumber"].map((route) => {
            return (
              <Route
                key={route}
                path={route}
                element={
                  <UserFeedsPage
                    userId={userId}
                    canEdit={canEdit}
                    pageNumber={pageNumber}
                    editorMode={editorMode}
                    onToggleCreateMode={onToggleCreateMode}
                  />
                }
              />
            );
          })}
          <Route path="/*" element={() => <div>NOT FOUND</div>} />
        </Routes>
      </div>
      <div className="col col-12 col-sm-12 col-md-12 col-lg-3 order-first order-lg-last mb-3">
        <ProfileInfo userInfo={userInfo} />
      </div>
    </div>
  );
};

export default Profile;
