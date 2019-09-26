﻿namespace Coco.Common.Resources
{
    public static class MailTemplateResources
    {
        public const string USER_CONFIRMATION_SUBJECT = @"Chào mừng đến với {0}";
        public const string USER_CONFIRMATION_BODY = @"<div>
                        <p>Xin chào {0},</p>
                        <p>Cám ơn bạn đã đăng ký tài khoản tại {1}, </p>
                        <p>hãy nhấn vào <a target=""_blank"" href=""{2}""><b>ĐÂY</b></a> để xác nhận.</p>
                        <p></p>
                        <p>Chúc bạn có những giây phút thật thú vị cùng chúng tôi</p>
                        </div>";
    }
}
