export interface User {
  id: string;
  email: string;
  fullName: string;
  phoneNumber?: string;
  role?: string;
  token?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phoneNumber: string;
}

// بيانات المستخدم داخل الـ Response
export interface AuthUserData {
  email: string;
  fullName: string;
  token: string;
  role: string;
}

// استجابة تسجيل الدخول
export interface AuthResponse {
  success: boolean;
  message: string;
  data: AuthUserData;
  errors: Record<string, string[]>;
  timestamp: string;
}

// استجابة التسجيل
export interface RegisterResponse {
  success: boolean;
  message: string;
  errors: Record<string, string[]>;
  timestamp: string;
}