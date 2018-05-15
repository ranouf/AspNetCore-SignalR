export class MessageDto implements IMessageDto {
  content?: string | undefined;
  author?: UserDto | undefined;

  constructor(data?: IMessageDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.content = data["Content"];
      this.author = data["Author"] ? UserDto.fromJS(data["Author"]) : <any>undefined;
    }
  }

  static fromJS(data: any): MessageDto {
    data = typeof data === 'object' ? data : {};
    let result = new MessageDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["Content"] = this.content;
    data["Author"] = this.author ? this.author.toJSON() : <any>undefined;
    return data;
  }
}

export interface IMessageDto {
  content?: string | undefined;
  author?: UserDto | undefined;
}

export class PrivateMessageDto extends MessageDto implements IPrivateMessageDto {
  recipient?: UserDto | undefined;

  constructor(data?: IPrivateMessageDto) {
    super(data);
  }

  init(data?: any) {
    super.init(data);
    if (data) {
      this.recipient = data["Recipient"] ? UserDto.fromJS(data["Recipient"]) : <any>undefined;
    }
  }

  static fromJS(data: any): PrivateMessageDto {
    data = typeof data === 'object' ? data : {};
    let result = new PrivateMessageDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["Recipient"] = this.recipient ? this.recipient.toJSON() : <any>undefined;
    super.toJSON(data);
    return data;
  }
}

export interface IPrivateMessageDto extends IMessageDto {
  recipient?: UserDto | undefined;
}

export class UserDto implements IUserDto {
  id?: string | undefined;
  name?: string | undefined;

  constructor(data?: IUserDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.id = data["Id"];
      this.name = data["Name"];
    }
  }

  static fromJS(data: any): UserDto {
    data = typeof data === 'object' ? data : {};
    let result = new UserDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["Id"] = this.id;
    data["Name"] = this.name;
    return data;
  }
}

export interface IUserDto {
  id?: string | undefined;
  name?: string | undefined;
}

export class UsersDto implements IUsersDto {
  user?: UserDto | undefined;
  users?: UserDto[] | undefined;

  constructor(data?: IUsersDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.user = data["User"] ? UserDto.fromJS(data["User"]) : <any>undefined;
      if (data["Users"] && data["Users"].constructor === Array) {
        this.users = [];
        for (let item of data["Users"])
          this.users.push(UserDto.fromJS(item));
      }
    }
  }

  static fromJS(data: any): UsersDto {
    data = typeof data === 'object' ? data : {};
    let result = new UsersDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["User"] = this.user ? this.user.toJSON() : <any>undefined;
    if (this.users && this.users.constructor === Array) {
      data["Users"] = [];
      for (let item of this.users)
        data["Users"].push(item.toJSON());
    }
    return data;
  }
}

export interface IUsersDto {
  user?: UserDto | undefined;
  users?: UserDto[] | undefined;
}

export class ErrorDto implements IErrorDto {
  message?: string | undefined;
  recipient?: UserDto | undefined;

  constructor(data?: IErrorDto) {
    if (data) {
      for (var property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  init(data?: any) {
    if (data) {
      this.message = data["Message"];
      this.recipient = data["Recipient"] ? UserDto.fromJS(data["Recipient"]) : <any>undefined;
    }
  }

  static fromJS(data: any): ErrorDto {
    data = typeof data === 'object' ? data : {};
    let result = new ErrorDto();
    result.init(data);
    return result;
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data["Message"] = this.message;
    data["Recipient"] = this.recipient ? this.recipient.toJSON() : <any>undefined;
    return data;
  }
}

export interface IErrorDto {
  message?: string | undefined;
  recipient?: UserDto | undefined;
}
