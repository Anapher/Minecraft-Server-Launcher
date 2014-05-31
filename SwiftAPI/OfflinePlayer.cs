/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace org.phybros.thrift
{

  /// <summary>
  /// Represents an offline player (or one that has never joined this server)
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class OfflinePlayer : TBase
  {
    private string _name;
    private long _firstPlayed;
    private long _lastPlayed;
    private bool _isOp;
    private bool _isBanned;
    private bool _isWhitelisted;
    private Player _player;
    private bool _hasPlayedBefore;

    /// <summary>
    /// The player's name
    /// </summary>
    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        this._name = value;
      }
    }

    /// <summary>
    /// The time/date the the player first joined (UNIX-timestamp style). 0 if never.
    /// </summary>
    public long FirstPlayed
    {
      get
      {
        return _firstPlayed;
      }
      set
      {
        __isset.firstPlayed = true;
        this._firstPlayed = value;
      }
    }

    /// <summary>
    /// The time/date the the player last joined (UNIX-timestamp style) 0 if never.
    /// </summary>
    public long LastPlayed
    {
      get
      {
        return _lastPlayed;
      }
      set
      {
        __isset.lastPlayed = true;
        this._lastPlayed = value;
      }
    }

    /// <summary>
    /// If the player is currently opped
    /// </summary>
    public bool IsOp
    {
      get
      {
        return _isOp;
      }
      set
      {
        __isset.isOp = true;
        this._isOp = value;
      }
    }

    /// <summary>
    /// If the player is currently banned from the server
    /// </summary>
    public bool IsBanned
    {
      get
      {
        return _isBanned;
      }
      set
      {
        __isset.isBanned = true;
        this._isBanned = value;
      }
    }

    /// <summary>
    /// If the player is currently on the server's whitelist
    /// </summary>
    public bool IsWhitelisted
    {
      get
      {
        return _isWhitelisted;
      }
      set
      {
        __isset.isWhitelisted = true;
        this._isWhitelisted = value;
      }
    }

    /// <summary>
    /// If the player is online, more information is held in this Player object
    /// </summary>
    public Player Player
    {
      get
      {
        return _player;
      }
      set
      {
        __isset.player = true;
        this._player = value;
      }
    }

    /// <summary>
    /// If the player has joined the server at least once before now
    /// </summary>
    public bool HasPlayedBefore
    {
      get
      {
        return _hasPlayedBefore;
      }
      set
      {
        __isset.hasPlayedBefore = true;
        this._hasPlayedBefore = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool name;
      public bool firstPlayed;
      public bool lastPlayed;
      public bool isOp;
      public bool isBanned;
      public bool isWhitelisted;
      public bool player;
      public bool hasPlayedBefore;
    }

    public OfflinePlayer() {
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.I64) {
              FirstPlayed = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I64) {
              LastPlayed = iprot.ReadI64();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.Bool) {
              IsOp = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.Bool) {
              IsBanned = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.Bool) {
              IsWhitelisted = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 7:
            if (field.Type == TType.Struct) {
              Player = new Player();
              Player.Read(iprot);
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.Bool) {
              HasPlayedBefore = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("OfflinePlayer");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      if (__isset.firstPlayed) {
        field.Name = "firstPlayed";
        field.Type = TType.I64;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(FirstPlayed);
        oprot.WriteFieldEnd();
      }
      if (__isset.lastPlayed) {
        field.Name = "lastPlayed";
        field.Type = TType.I64;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(LastPlayed);
        oprot.WriteFieldEnd();
      }
      if (__isset.isOp) {
        field.Name = "isOp";
        field.Type = TType.Bool;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(IsOp);
        oprot.WriteFieldEnd();
      }
      if (__isset.isBanned) {
        field.Name = "isBanned";
        field.Type = TType.Bool;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(IsBanned);
        oprot.WriteFieldEnd();
      }
      if (__isset.isWhitelisted) {
        field.Name = "isWhitelisted";
        field.Type = TType.Bool;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(IsWhitelisted);
        oprot.WriteFieldEnd();
      }
      if (Player != null && __isset.player) {
        field.Name = "player";
        field.Type = TType.Struct;
        field.ID = 7;
        oprot.WriteFieldBegin(field);
        Player.Write(oprot);
        oprot.WriteFieldEnd();
      }
      if (__isset.hasPlayedBefore) {
        field.Name = "hasPlayedBefore";
        field.Type = TType.Bool;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(HasPlayedBefore);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("OfflinePlayer(");
      sb.Append("Name: ");
      sb.Append(Name);
      sb.Append(",FirstPlayed: ");
      sb.Append(FirstPlayed);
      sb.Append(",LastPlayed: ");
      sb.Append(LastPlayed);
      sb.Append(",IsOp: ");
      sb.Append(IsOp);
      sb.Append(",IsBanned: ");
      sb.Append(IsBanned);
      sb.Append(",IsWhitelisted: ");
      sb.Append(IsWhitelisted);
      sb.Append(",Player: ");
      sb.Append(Player== null ? "<null>" : Player.ToString());
      sb.Append(",HasPlayedBefore: ");
      sb.Append(HasPlayedBefore);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
