// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;

namespace SharpDX.DirectSound
{
    public partial class DirectSound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectSound"/> class.
        /// </summary>
        public DirectSound() : base(IntPtr.Zero)
        {
            DirectSound temp;
            DSound.Create8(null, out temp, null);
            NativePointer = temp.NativePointer;
        }

        /// <summary>
        /// Verifies the certification.
        /// </summary>
        /// <returns>Return true if the driver is certified</returns>
        public bool IsCeritified
        {
            get
            {
                int verify;
                this.VerifyCertification(out verify);
                return verify == 0;
            }
        }

        /// <summary>
        /// Retrieves the speaker configuration of the device.
        /// </summary>
        /// <param name="speakerSet" />
        /// <param name="geometry" />
        public Result GetSpeakerConfiguration(out SpeakerConfiguration speakerSet, out SpeakerGeometry geometry)
        {
            int speakerConfig;
            Result result = GetSpeakerConfiguration(out speakerConfig);
            speakerSet = (SpeakerConfiguration)(speakerConfig & 0xFFFF);
            geometry = (SpeakerGeometry)(speakerConfig >> 16);
            return result;            
        }

        /// <summary>
        /// Sets the speaker configuration of the device.
        /// </summary>
        /// <param name="speakerSet" />
        /// <param name="geometry" />
        public Result SetSpeakerConfiguration(SpeakerConfiguration speakerSet, SpeakerGeometry geometry)
        {
            return SetSpeakerConfiguration(((int)speakerSet) | (((int)geometry) << 16));
        }

        /// <summary>
        /// Enumerates the DirectSound devices installed in the system.
        /// </summary>
        /// <returns>A collection of the devices found.</returns>
        public static List<DeviceInformation> GetDevices()
        {
            EnumDelegateCallback callback = new EnumDelegateCallback();
            DSound.EnumerateW(callback.NativePointer, IntPtr.Zero);
            return callback.Informations;
        }
    }
}