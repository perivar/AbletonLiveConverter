using System;
using System.Collections.Generic;

namespace PresetConverterProject.NIKontaktNKS
{
    public static class NKSLibraries
    {
        public static List<NksLibraryDesc> Libraries =
        new List<NksLibraryDesc>
        {
          new NksLibraryDesc {
            Id = 0x0000000d, Name = "Keyboard Collection", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60 ,0xa2, 0x19, 0x2b},
              IV = new byte[] { 0x60, 0xda, 0xb1, 0xcb },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000065, Name = "Stradivari Solo Violin", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xe8, 0xdf, 0xe4 },
              IV = new byte[] { 0x60, 0x12, 0x23, 0x04 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000067, Name = "OTTO", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x32, 0x29, 0x97 },
              IV = new byte[] { 0x60, 0x63, 0x20, 0x37 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000068, Name = "Acoustic Legends HD", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x7c, 0x07, 0xf8 },
              IV = new byte[] { 0x60, 0x38, 0x2d, 0x18 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000069, Name = "Ambience Impacts Rhythms", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x86, 0xdf, 0x59 },
              IV = new byte[] { 0x60, 0x18, 0xb2, 0xf9 },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000006a, Name = "Chris Hein - Guitars", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xa3, 0xa0, 0xaa },
              IV = new byte[] { 0x60, 0xa0, 0x3a, 0xca },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000006b, Name = "Solo Strings Advanced", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xa3, 0xf3, 0x0a },
              IV = new byte[] { 0x60, 0xec, 0xfd, 0x2a },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000006f, Name = "Drums Overkill", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x11, 0x14, 0xaa },
              IV = new byte[] { 0x60, 0xdf, 0xae, 0xca },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000073, Name = "VI.ONE", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x78, 0x29, 0xaf },
              IV = new byte[] { 0x60, 0x3a, 0xfc, 0x4f },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000074, Name = "Gofriller Cello", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x7d, 0x40, 0xe3 },
              IV = new byte[] { 0x60, 0x44, 0x45, 0x83 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000195, Name = "Evolve Mutations", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x4b, 0x2b, 0x29 },
              IV = new byte[] { 0x60, 0x47, 0x46, 0xc9 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000320, Name = "syntAX", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x1d, 0xfa, 0x21 },
              IV = new byte[] { 0x60, 0x9d, 0xa1, 0xc1 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000322, Name = "Galaxy II", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x2f, 0x74, 0x98 },
              IV = new byte[] { 0x60, 0x40, 0xa9, 0xb8 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000327, Name = "Garritan Instruments for Finale", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x55, 0x73, 0xde },
              IV = new byte[] { 0x60, 0x3c, 0x3f, 0xfe },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000033b,  Name = "Mr. Sax T", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x26, 0x7a, 0x9a },
              IV = new byte[] { 0x60, 0x50, 0x2c, 0xba },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000033c, Name = "The Trumpet", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xc2, 0x9d, 0xc0 },
              IV = new byte[] { 0x60, 0x74, 0x16, 0xe0 },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000033d, Name = "Prominy SC Electric Guitar", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x97, 0x26, 0x2b },
              IV = new byte[] { 0x60, 0xfc, 0x3e, 0xcb },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000340, Name = "The Elements", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xa7, 0x2a, 0x48 },
              IV = new byte[] { 0x60, 0x6b, 0xd7, 0x68 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000341, Name = "Phaedra", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xc3, 0x3c, 0x08 },
              IV = new byte[] { 0x60, 0xbd, 0x49, 0x28 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000343, Name = "String Essentials", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xf0, 0xf3, 0xfe },
              IV = new byte[] { 0x60, 0xaf, 0x90, 0x1e },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000344, Name = "Ethno World 4", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xfb, 0xb3, 0xb5 },
              IV = new byte[] { 0x60, 0x53, 0xfd, 0x55 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000345, Name = "Chris Hein Bass", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x13, 0x65, 0x88 },
              IV = new byte[] { 0x60, 0xef, 0x32, 0xa8 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000349, Name = "Vir2 Elite Orchestral Percussion", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xf1, 0x57, 0x30 },
              IV = new byte[] { 0x60, 0xa0, 0xa8, 0x50 },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000034a, Name = "BASiS", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x87, 0x67, 0x5d },
              IV = new byte[] { 0x60, 0x90, 0x34, 0xfd },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000034f, Name = "Ocean Way Drums Gold", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x18, 0xab, 0x90 },
              IV = new byte[] { 0x60, 0x11, 0x6c, 0xb0 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000351, Name = "Evolve", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xea, 0x44, 0xd5 },
              IV = new byte[] { 0x60, 0x40, 0xde, 0x75 },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000355, Name = "Kreate", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x50, 0xe2, 0x5f },
              IV = new byte[] { 0x60, 0x82, 0xac, 0xff },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000356, Name = "Symphobia", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x5f, 0x52, 0xc5 },
              IV = new byte[] { 0x60, 0x95, 0x04, 0x65 },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000035e, Name = "Ocean Way Drums Expandable", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x2d, 0x19, 0x0e },
              IV = new byte[] { 0x60, 0x80, 0x1d, 0x2e },
            }
          },
          new NksLibraryDesc {
            Id = 0x00000360, Name = "Steven Slate Drums Platinum", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0x67, 0x52, 0x8e },
              IV = new byte[] { 0x60, 0x84, 0x16, 0xae },
            }
          },
          new NksLibraryDesc {
            Id = 0x0000038a, Name = "Chris Hein Horns Vol 2", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xc8, 0xf7, 0xa9 },
              IV = new byte[] { 0x60, 0x72, 0x53, 0x49 },
            }
          },
          new NksLibraryDesc {
            Id = 709, Name = "Neo-Soul Keys", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0xAB, 0x90, 0x62, 0x62, 0x5F, 0x0C, 0x27, 0x75, 0x2B, 0x5C, 0x2A, 0xC8, 0x19, 0x1A, 0xB3, 0x1E, 0xDA, 0x72, 0x07, 0x42, 0xCB, 0x0B, 0x22, 0xF5, 0xB4, 0x5E, 0xB8, 0x96, 0xB9, 0x9C, 0x0B, 0xD2 },
              IV = new byte[] { 0xA7, 0xFA, 0xC4, 0x1D, 0x35, 0x21, 0x49, 0x59, 0x8D, 0x91, 0xE6, 0x0D, 0xAE, 0xF9, 0x99, 0xDE },
            }
          },
          new NksLibraryDesc {
            Id = 0x00001388, Name = "UserPatches", GenKey = new NksGeneratingKey {
              Key = new byte[] { 0x60, 0xd0, 0xde, 0x83 },
              IV = new byte[] { 0x60, 0x63, 0x73, 0x23 },
            }
          },
      };
    }

    public class NksLibraryDesc
    {
        public UInt32 Id { get; set; }
        public string Name { get; set; }
        public NksGeneratingKey GenKey = new NksGeneratingKey();
    }

    public class NksGeneratingKey
    {
        public byte[] Key { get; set; }
        public int KeyLength { get { return Key != null ? Key.Length : 0; } }
        public byte[] IV { get; set; }
        public int IVLength { get { return IV != null ? IV.Length : 0; } }
    }
}