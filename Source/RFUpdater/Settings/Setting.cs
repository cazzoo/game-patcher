// RFUpdater is a game module updater that allows teams to create, manage and get folders synched.
// Copyright (C) 2016 - //Racing-France//
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
namespace RFUpdater
{
	public class Setting
	{
		public enum SettingType
		{
			TEXT,
			NUMBER,
			PATH,
			SPINNER,
			DROPDOWN,
			TOGGLE
		}

		public string Name;
		public string Value;
		public string DefaultValue = string.Empty;
		public string Category;
		public SettingType Type;
		public bool Writable;

		public Setting (string pName, string pValue, string pCategory, SettingType pType = SettingType.TEXT, bool pWritable = true)
		{
			Name = pName;
			Value = pValue;
			DefaultValue = Value;
			Category = pCategory;
			Type = pType;
			Writable = pWritable;
		}
	}
}
